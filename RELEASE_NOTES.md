# DARIA.EXE 3.7.0 — Actual Game Loop

## What changed

Quick Play is no longer a multiple-choice lesson.

- Removed all pre-choice quality hints and "teacher answer" labels.
- Replaced good/weak/bad cards with four plausible actions from the case itself.
- Added a visible case board with limited moves and clue slots.
- Each action reveals information and has a world-side-effect: actors may be alerted, time passes, comparison narrows the field, or a test changes the system.
- Actions cannot be repeated in the same quick case.
- Players can decide early or spend the full move budget.
- Final calls use the case's real decision options instead of auto-scoring the last move.
- Scoring moved to the debrief and now considers evidence usefulness, coverage, commitment and the final call.
- Daria no longer tells the player whether a move was "correct" during play.

## Release gate

Ship only after the real Chromium/WebKit/device suite passes the new case-board interaction and confirms that no pre-choice answer hints remain.
