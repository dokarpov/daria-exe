# DARIA.EXE v3.7 — Actual Game Loop

DARIA.EXE is a reasoning game about making decisions under uncertainty.

## Quick Play

Quick Play is now a real case-board loop rather than a quiz:

- four plausible actions;
- three-move budget;
- every action reveals a clue and changes the situation;
- no "correct answer" hints before choosing;
- actions cannot be repeated;
- the player can decide early or keep investigating;
- final calls are judged only in the debrief.

## Investigation

The deeper mode keeps the five-move hypothesis/evidence loop, swipe/tap classification, contradictions and early-call option.

## Validation

The release uses real HTTP-served Playwright E2E on Chromium, WebKit, iPhone, Android and iPad profiles. v3.7 adds explicit checks that Quick Play contains no pre-choice answer-quality hints and that clue/action-state mechanics work.
