# Changelog

All notable changes to this project are documented here. Format loosely follows
[Keep a Changelog](https://keepachangelog.com/).

## [Unreleased]
- Reviewer-facing v1 vs v2 diff badge on the Dashboard.
- First-time-cloner README walkthrough with a captured screenshot.

## v2 hardening (2026, merged to main)
- Rename PlayerWallet to GrainWallet and patch a MessagePack CVE (#28).
- Flatten v1 and v2 source into committed folders on `main` (#22).
- `main` becomes the hub: Dashboard only, v1/v2 source moved in from branches (#21).
- v2: Postgres outbox with `FOR UPDATE SKIP LOCKED` drain, real LRU idempotency, HTTP 503
  back-pressure gate, `synchronous_commit=on`, currency `VARCHAR(3)` + CHECK, event rename
  `DeductionRejected` -> `OperationRejected`.
- Standard CI, security scan and monthly Dependabot actions added (#29).

## submission-v1 (tag)
- Original GrainWallet submission: in-memory LRU idempotency, basic outbox drainer.
