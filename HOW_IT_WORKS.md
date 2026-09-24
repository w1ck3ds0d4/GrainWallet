# How it works

Think of GrainWallet as two identical restaurant kitchens (v1 and v2) cooking from the same order
ticket, with a food critic (the Dashboard) timing both and writing up which one plated faster.

## Walking through one comparison run

1. **You start both kitchens.** Running the `Compare: v1 + v2 + Dashboard` compound launches v1's
   API on port 5000, v2's on port 5001, and the Dashboard on port 5100. v1 keeps its books in
   memory; v2 keeps them in a real Postgres ledger and posts receipts to a Kafka message board.

2. **You click Run in the Dashboard.** NBomber (a load-testing library) fires a batch of wallet
   operations, `add-funds`, `deduct-funds`, `get-balance`, at both APIs at the same time, using the
   same scenario for each.

3. **Each API hands the work to a grain.** A "grain" (Orleans' term) is a tiny, single-threaded
   virtual actor, one per player wallet. Because only one grain instance ever owns a given wallet
   at a time, there is no race condition between two requests hitting the same balance, no locking
   code needed.

4. **The two kitchens record the change differently.** v1 writes the resulting event to an
   in-memory queue and drains it on a timer, good enough for a demo, but a crash loses anything
   still queued. v2 writes the event to a Postgres table in the same transaction as the balance
   change, then a separate drainer picks it up with `FOR UPDATE SKIP LOCKED` (a way for several
   drainers to grab different rows without stepping on each other) and publishes it to Kafka. If
   Postgres commits, the event is guaranteed to exist somewhere, even if Kafka is briefly down.

5. **v2 also knows when to say no.** Under heavy load it returns HTTP 503 instead of queuing
   endless work, a back-pressure gate. v1 has no such gate, so it will happily fall further and
   further behind instead of pushing back.

6. **The Dashboard tallies both runs.** Latency percentiles (p50/p95/p99), throughput and error
   rates land side by side, so you can see concretely what the v2 hardening bought: not just
   "it's better" but by how much, and where it costs a 503 instead of a slow success.

## When to reach for this repo

Use it to demonstrate, to yourself or an interviewer, the practical difference between a
quick-and-dirty service and the same service hardened for real concurrent writes and durable
messaging, backed by numbers instead of a slide.
