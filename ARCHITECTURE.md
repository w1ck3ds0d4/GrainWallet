# Architecture

## Components

**Hub (`main`, `src/GrainWallet.Dashboard/`)**
A single ASP.NET Core app. `BenchRunner`/`BenchScenarios`/`SuiteRunner` drive NBomber load against
one or more running wallet APIs; `DashboardEndpoints` exposes the run/results API; `wwwroot/`
is the static comparison UI. `appsettings.json` maps a project name (`v1`, `v2`, ...) to the base
URL where that version's API is listening.

**Version trees (`v1/`, `v2/`, frozen at their snapshot SHA)**
Each is a full, independently runnable Orleans service, orchestrated by its own .NET Aspire
`AppHost`:

- `GrainWallet.Contracts` - the `IWalletGrain` interface, `Money`, `WalletOperations`, wallet events.
- `GrainWallet.Grains` - the `WalletGrain` implementation and its state store.
- `GrainWallet.Api` - the HTTP endpoints and wiring (Postgres, Kafka, outbox) that front the grains.
- `GrainWallet.AppHost` - Aspire orchestration: starts Postgres + Kafka (v2) and the API together.
- `GrainWallet.ServiceDefaults` - shared OpenTelemetry/health-check wiring.

**v1 vs v2, what actually changed**
v1 keeps wallet state in-memory with a basic LRU for idempotency and a simple outbox drainer.
v2 hardens the same shape: Postgres row locking via `FOR UPDATE SKIP LOCKED` on outbox drain
(`PostgresWalletStateStore`, `WalletOutboxDrainer`), a real LRU-backed idempotency check, an HTTP
503 back-pressure gate under load, `synchronous_commit=on`, a `VARCHAR(3)` + CHECK constraint on
currency, and an event rename (`DeductionRejected` -> `OperationRejected`). Kafka publishing
(`KafkaWalletEventPublisher`) is real in v2; v1 and a no-op variant exist for comparison.

## Data flow (a single "add funds" call, compared)

1. The Dashboard's `BenchRunner` fires an HTTP request at a version's `WalletEndpoints` (v1 or v2).
2. The API resolves the target `WalletGrain` by player id (Orleans single-threaded grain activation
   guarantees no races within one wallet).
3. The grain applies the operation to `WalletState` and appends a domain event.
4. v1: the event goes to an in-memory outbox, drained on a timer, with no-op or Kafka publish.
   v2: the event is written to the Postgres outbox in the same transaction as the state change,
   then drained with `FOR UPDATE SKIP LOCKED` so multiple drainers cannot double-publish, and
   published to Kafka.
5. NBomber records the latency and outcome (including 503s from v2's back-pressure gate) back to
   the Dashboard, which aggregates both versions' `NodeStats` and renders the comparison.

## Why this shape

Comparing two service versions honestly means running both, unmodified, against the same load at
the same time, not replaying recorded numbers. Freezing `v1/`/`v2/` as committed source (rather
than branches or tags) keeps a single `git clone` sufficient to reproduce the comparison, which is
the point of the repo as an interview artifact.
