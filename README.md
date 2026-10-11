# DARIA.EXE

**A game about making good decisions before certainty arrives.**

DARIA.EXE turns messy, ambiguous situations into short playable cases. You have limited moves, incomplete evidence, competing explanations, and a decision to make before the clock — or your attention — runs out.

**Play:** https://dokarpov.github.io/daria-exe/

## How it plays

### Quick Play
A 1–2 minute case built around a constrained decision.

- 4 plausible actions
- 3-move investigation budget
- every move reveals a clue and may change the situation
- no pre-choice “correct answer” hints
- decide early or keep investigating
- scoring happens only after the call

### Investigation
A deeper 5–10 minute mode.

- competing hypotheses
- limited research actions
- evidence can support, contradict, or be irrelevant
- contradictions weaken hypotheses visually
- make the call whenever the remaining uncertainty is acceptable

## What the game teaches

Not “find the right answer.”

It trains the harder skill: **choose the next move that is most likely to change the decision.**

Cases are built around recurring reasoning traps such as hidden constraints, biased evidence, wrong comparisons, incentive mismatches, multiple causes, time pressure, and irreversible decisions.

## Current release

**v3.7 — Actual Game Loop**

This release replaces the earlier quiz-like Quick Play with a real case-board loop: plausible actions, hidden information, limited moves, consequences, early decisions, and debrief-only scoring.

## Validation

The production build is tested over HTTP with Playwright across:

- Chromium
- WebKit
- iPhone profile
- Android profile
- iPad profile
- narrow mobile widths and landscape

The suite also checks persistence, evidence gating, contradiction states, touch targets, keyboard focus, reduced motion, and horizontal overflow.

## Project status

Active prototype. Gameplay and case quality are still being iterated aggressively.

---
Built as an experimental reasoning game by Daria Karpova.
