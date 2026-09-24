# Contributing to GrainWallet

This is a portfolio/interview artifact rather than an actively maintained product, but pull
requests are welcome for fixes, clarity, or a new version tree.

## Setup

```bash
git clone https://github.com/w1ck3ds0d4/GrainWallet.git
cd GrainWallet
dotnet restore GrainWallet.slnx
```

Requires .NET 10 SDK. Running a full comparison (`v1/`, `v2/`) additionally needs Docker, since
v2's Aspire AppHost starts local Postgres and Kafka containers.

## Development

```bash
dotnet format GrainWallet.slnx --verify-no-changes --no-restore --severity error
dotnet build GrainWallet.slnx -c Release --no-restore -warnaserror
dotnet test GrainWallet.slnx -c Release --no-build
```

Or use the VS Code compound `Compare: v1 + v2 + Dashboard` to run everything together.

## Adding a new version (v3, v4, ...)

`v1/` and `v2/` are frozen at their snapshot SHA and are never edited in place. To add a new
version:

1. Copy an existing version folder's structure into `vN/`, keeping the same project shapes
   (`Api`, `AppHost`, `Contracts`, `Grains`, `ServiceDefaults`) so the Dashboard's assumptions hold.
2. Give it its own port block in `appsettings.json`/environment variables so it can run alongside
   the others.
3. Add it to `src/GrainWallet.Dashboard/appsettings.json`'s `Projects` list.
4. Update the README's version table and the Compare compound in `.vscode/`.

## Code standards

- Commit messages use a `(type)` prefix: `(feat)`, `(fix)`, `(refactor)`, `(docs)`, `(chore)`.
- No em dashes or en dashes anywhere, in code, comments, commits or docs.
- `dotnet format` and `-warnaserror` both gate CI; a PR with formatting or warning issues will fail.

## License

GrainWallet is dual-licensed:

- [AGPL v3](LICENSE) for open-source use. Derivatives and hosted deployments must release their
  source under AGPL.
- [Commercial license](COMMERCIAL.md) for proprietary or closed-source use that does not want to
  comply with AGPL source-disclosure requirements.

By submitting a pull request you agree to license your contribution under both terms.
