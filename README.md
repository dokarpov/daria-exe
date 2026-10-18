# DARIA.EXE v3.8 — Incident-first browser release

[![Browser validation](https://github.com/dokarpov/daria-exe/actions/workflows/browser-validation.yml/badge.svg)](https://github.com/dokarpov/daria-exe/actions/workflows/browser-validation.yml)

**A consumer reasoning game about making decisions under uncertainty.**

[Play it live →](https://dokarpov.github.io/daria-exe/)

DARIA.EXE is deliberately different from the systems/enterprise projects in this portfolio. It tests whether decision quality can become a **playable interaction**, not another dashboard or productivity tool.

**Release boundary:** the public link opens the validated browser-based case game. The more immersive Unity WebGL version is a separate development track; it is **not** the live release or a verified working build. Do not conflate browser test coverage with Unity release acceptance.

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
| Retention / repeat play | **not yet proven** |
| Learning or decision-quality improvement | **not claimed** |
| Monetisation | **not claimed** |

The current proof is therefore **interaction quality + working game mechanics**, not consumer traction.

### Technology status

- **Browser release:** primary shipping track. Fully testable in CI and deployable to GitHub Pages.
- **Unity WebGL:** experimental R&D track. Art and interaction scaffolding exist, but no WebGL output is accepted as a release until it is built and played successfully.
- **Migration rule:** do not replace the browser product merely for engine prestige. A migration must improve world interaction, animation or authoring enough to justify the added build/runtime cost. The next meaningful product test is whether people voluntarily complete, replay and share cases.

For the next player-evidence stage and the boundary between engagement and learning claims, see [`VALIDATION.md`](VALIDATION.md).

## Why it belongs in the portfolio

It demonstrates a different product muscle from the agentic/control-plane work: consumer interaction design, stateful game mechanics, information pacing and designing uncertainty without leaking the “right” answer through the UI.

---

Built by **Daria Karpova** · Product Systems · AI · 0→1 · [dokarpov](https://github.com/dokarpov)
