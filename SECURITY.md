# Security

GrainWallet is a local comparison harness, not a deployed service. Nothing in this repo is meant
to run on the public internet.

## What the code touches

- **Network**: the Dashboard and both version APIs bind to localhost ports (5000, 5001, 5100) for
  local runs only. v2's `AppHost` also starts a local Postgres and Kafka via .NET Aspire, both
  ephemeral, dev-only containers with no exposed credentials beyond their local dev defaults.
- **Secrets**: none. No API keys, connection strings to real infrastructure, or auth tokens are
  used anywhere in the repo. Aspire generates its own local dev connection strings at run time.
- **Files**: no persistent storage outside the Aspire-managed containers, which are torn down when
  the compound stops.
- **Permissions**: the CI workflow runs with `contents: read` only; it builds, formats and tests,
  and does not push, comment, or hold write access.

## Known risks

- **`v1/` and `v2/` are frozen source, not maintained services.** Their dependencies are not meant
  to be run in production and are not patched for anything beyond keeping the demo buildable.
  Treat both as fixtures, not as code to deploy.
- **v1 has no back-pressure gate.** Under sustained load it queues indefinitely rather than
  rejecting requests, this is the entire point of the comparison, not a bug to fix in v1.
- **Aspire's local Postgres/Kafka use default dev credentials.** Fine for a throwaway local
  container; never reuse those defaults for anything that leaves your machine.

## Reporting a problem

Use GitHub's private vulnerability reporting on this repo (Security tab -> Report a vulnerability),
or email daniel.svs@outlook.com. Since this is a local-only demo harness with no production
deployment, most findings will be about the code as a teaching example rather than a live attack
surface, but real dependency vulnerabilities are still welcome as reports.
