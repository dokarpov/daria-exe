# DARIA.EXE v3.7 — Actual Game Loop

[![Browser validation](https://github.com/dokarpov/daria-exe/actions/workflows/browser-validation.yml/badge.svg)](https://github.com/dokarpov/daria-exe/actions/workflows/browser-validation.yml)

**A consumer reasoning game about making decisions under uncertainty.**

[Play it live →](https://dokarpov.github.io/daria-exe/)

DARIA.EXE is deliberately different from the systems/enterprise projects in this portfolio. It tests whether decision quality can become a **playable interaction**, not another dashboard or productivity tool.

## Product loop

### Quick Play

Quick Play is a real case-board loop rather than a quiz:

- four plausible actions;
- three-move budget;
- every action reveals a clue and changes the situation;
- no “correct answer” hints before choosing;
- actions cannot be repeated;
- the player can decide early or keep investigating;
- final calls are judged only in the debrief.

### Investigation

The deeper mode keeps the five-move hypothesis/evidence loop, swipe/tap classification, contradictions and early-call option.

## Evidence today

| Evidence | Status |
|---|---|
| Playable public product | live |
| Quick Play state loop | implemented |
| Investigation loop | implemented |
| No pre-choice answer-quality hints | covered by browser tests |
| Chromium / WebKit / iPhone / Android / iPad browser validation | passing |
| Retention / repeat play | **not yet proven** |
| Learning or decision-quality improvement | **not claimed** |
| Monetisation | **not claimed** |

The current proof is therefore **interaction quality + working game mechanics**, not consumer traction. The next meaningful product test is whether people voluntarily complete, replay and share cases.

For the next player-evidence stage and the boundary between engagement and learning claims, see [`VALIDATION.md`](VALIDATION.md).

## Why it belongs in the portfolio

It demonstrates a different product muscle from the agentic/control-plane work: consumer interaction design, stateful game mechanics, information pacing and designing uncertainty without leaking the “right” answer through the UI.

---

Built by **Daria Karpova** · Product Systems · AI · 0→1 · [dokarpov](https://github.com/dokarpov)
