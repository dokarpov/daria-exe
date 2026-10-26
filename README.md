# DARIA.EXE v3.8 — Incident-first browser release

[![Browser validation](https://github.com/dokarpov/daria-exe/actions/workflows/browser-validation.yml/badge.svg)](https://github.com/dokarpov/daria-exe/actions/workflows/browser-validation.yml)

**A consumer reasoning game about making decisions under uncertainty.**

[Play it live →](https://dokarpov.github.io/daria-exe/)

DARIA.EXE is deliberately different from the systems/enterprise projects in this portfolio. It tests whether decision quality can become a **playable interaction**, not another dashboard or productivity tool.

**Stage:** Playable browser game. Unity is a separate R&D track.

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
| Playable public product | live after v3.8 promotion |
| Quick Play state loop | implemented |
| Investigation loop | implemented |
| No pre-choice answer-quality hints | covered by browser tests |
| Chromium / WebKit / iPhone / Android / iPad browser validation | release gate |

**Next evidence:** voluntary completion, repeat play and learning outcomes.

### Technology status

- **Browser release:** primary shipping track. Fully testable in CI and deployable to GitHub Pages.
- **Unity WebGL:** separate exploration of richer animation and interaction.
- **Migration rule:** do not replace the browser product merely for engine prestige. A migration must improve world interaction, animation or authoring enough to justify the added build/runtime cost. The next meaningful product test is whether people voluntarily complete, replay and share cases.

See [`VALIDATION.md`](VALIDATION.md) for player research and release criteria.

## Why it belongs in the portfolio

It demonstrates a different product muscle from the agentic/control-plane work: consumer interaction design, stateful game mechanics, information pacing and designing uncertainty without leaking the “right” answer through the UI.

---

Built by **Daria Karpova** · Product Systems · AI · 0→1 · [dokarpov](https://github.com/dokarpov)
