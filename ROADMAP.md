# Roadmap

**Status:** release: path to v1.0.0. **Last reviewed:** 2026-09-24.

GrainWallet is a finished comparison harness (hub layout, v1 and v2 source trees, a working
NBomber dashboard) built as an interview/portfolio artifact, not a live product. "Done" means a
tagged, documented `v1.0.0` that a stranger can clone and run in one sitting, after which it moves
to maintain.

> How this file is used: Claude Project threads build the first unticked item under **Now**, one item per branch and pull request, and tick it in that same PR as `- [x] ... (#PR)`. Daniel owns the order and the lists; threads never add to Now, Next or Later themselves, they propose under **Ideas**.

## Now (path to v1.0.0)
- [ ] **Smoke test the Compare flow on a clean machine**: fresh clone into a scratch directory, run the `Compare: v1 + v2 + Dashboard` compound, confirm v1 binds `:5000`, v2 binds `:5001`, Dashboard binds `:5100`, and each benchmark (`add-funds`, `deduct-funds`, `get-balance`) renders side-by-side cards. Done when: zero manual steps beyond `git clone` and the compound launch, and a screenshot is captured for the README.
- [ ] **README first-time-cloner walkthrough**: add the captured screenshot and confirm the existing run steps are enough with no tribal knowledge. Done when: someone who has never seen the repo can go from clone to a rendered comparison using only the README.
- [ ] **Reviewer-facing diff badge**: a "v1 vs v2 delta" tile on the Dashboard highlighting p95/p99 deltas as `+X% faster` / `-X% slower`, computed from in-memory `NodeStats`, plus v2's back-pressure 503 rate (invisible today). Done when: a reviewer can open the dashboard, click Run, and state v2's wins without reading raw numbers.
- [ ] **Tag v1.0.0**: bump the Dashboard `.csproj` to an informational 1.0.0 marker, push tag `v1.0.0` on the smoke-tested commit, and cut a GitHub release with the README's run steps in the notes. Done when: `git tag --list` shows `v1.0.0` and a GitHub release exists for it.

## Next
- [ ] **Per-version README linkbacks**: link `v1/README.md` and `v2/README.md` from the hub README so a reader can go straight to either version's own docs. Done when: both links resolve from the hub README.
- [ ] **Move to maintain**: after the tag, this repo needs no further roadmap cadence; keep CI green and Dependabot patched only.

## Later
- Adding v3 as the same committed-subfolder pattern, once a v3 branch exists.
- Charts in the Dashboard: per-scenario latency lines over benchmark history.
- Dashboard config UI for adjusting bench knobs without restarting.

## Ideas
(empty to start; threads add proposals here)

## Done
- [x] Hub identity: Dashboard-only `main`, `v1/`/`v2/` as committed source trees, single clone (#21, #22)
- [x] `Compare: v1 + v2 + Dashboard` VS Code compound works end-to-end
- [x] Dashboard `appsettings.json` maps project names to URLs
- [x] CI gates `main` on format + build + test
- [x] v1 source committed and runnable (`v1/`)
- [x] v2 source committed and runnable (`v2/`): hardened outbox, back-pressure gate, schema fixes, event rename (#28)
