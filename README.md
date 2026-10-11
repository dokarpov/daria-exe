# DARIA.EXE v3.5 — Gameplay Validation & Polish

A reasoning game for making better calls under uncertainty.

## Core game modes

- **Quick Play** — a 30–90 second case: choose → reveal → choose → call → debrief.
- **Investigation** — a deeper case with competing explanations, a five-move research budget, evidence classification and an explicit `Make a call` decision.

The modes share the same 24-case library, progression, Daria feedback and reasoning patterns.

## v3.5 release goal

v3.5 does not add another product subsystem. It hardens v3.4 as a real game:

- real HTTP-served Playwright E2E specification;
- Chromium + WebKit CI matrix;
- iPhone, Android and iPad profiles;
- reload/persistence and corrupted-state recovery;
- swipe evidence classification and exhausted-move paths;
- 320/360/390/430px, landscape and short-height checks;
- 44px touch-target gate;
- keyboard-focus and reduced-motion checks;
- 200% zoom stress;
- service-worker registration check;
- canonical visual captures;
- schema/consistency validation for all 24 cases.

See `VALIDATION_STATUS.md` for the distinction between tests executed in the build container and the authoritative browser CI gate.