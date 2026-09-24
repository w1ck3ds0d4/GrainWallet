# GrainWallet

A side-by-side comparison harness for a per-player wallet microservice (Orleans grains). `main`
carries the Dashboard hub; each numbered version (`v1/`, `v2/`) is a committed, frozen source tree
with its own Aspire AppHost, API and grain implementation. The Dashboard runs NBomber against all
of them in parallel and renders the latency comparison. Public, interview/portfolio artifact.

## Commands

```bash
dotnet restore GrainWallet.slnx
dotnet format GrainWallet.slnx --verify-no-changes --no-restore --severity error   # what CI runs
dotnet build GrainWallet.slnx -c Release --no-restore -warnaserror
dotnet test GrainWallet.slnx -c Release --no-build
dotnet run --project src/GrainWallet.Dashboard   # http://localhost:5100
```

Or use the VS Code compound: Run and Debug -> `Compare: v1 + v2 + Dashboard`, which builds both
AppHosts and the Dashboard together. `dotnet format` and `dotnet build -warnaserror` both gate CI;
run them before calling anything done.

## Layout

| Path | What it is |
| --- | --- |
| `src/GrainWallet.Dashboard/` | The hub app: NBomber benchmark runner, comparison UI (`wwwroot/`) |
| `v1/` | Frozen v1 source: in-memory LRU idempotency, basic outbox drainer |
| `v2/` | Frozen v2 source: `FOR UPDATE SKIP LOCKED` outbox, back-pressure gate, hardened schema |
| `GrainWallet.slnx` | Hub solution; only builds the Dashboard, not `v1/`/`v2/` |
| `.vscode/` | The Compare compound and per-version build tasks |

## Conventions

- Commit format: `(type) lowercase summary`.
- ASCII hyphens only, no em dashes or en dashes.
- Branch + PR per change; CI (`dotnet format` + build + test) must be green before merge.
- `v1/` and `v2/` stay frozen at their snapshot SHA. Never change code inside them; a new version
  is added as `v3/` etc, never a rewrite of an existing one.

## Do not read

`**/bin/`, `**/obj/`, `*.trx` test result files.
