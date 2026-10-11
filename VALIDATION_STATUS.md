# v3.5 validation status

## RELEASE GATE: PASSED

The production build was served over HTTP in GitHub Actions and the real Playwright suite passed before merge on commit `1331828c23f2c3432eeecc5d82fef65dca404e6f`.

Validated browser profiles:
- Desktop Chromium
- Desktop WebKit
- iPhone WebKit
- Android Chromium
- iPad WebKit

Validated behavior includes:
- Quick Play completion and scoring
- XP / streak / history persistence across reload
- corrupted localStorage recovery
- Investigation action gating while evidence is unresolved
- tap and swipe evidence classification
- contradiction state
- early Make a Call
- 320 / 360 / 390 / 430 px, landscape and short-height overflow checks
- 44 px touch targets
- keyboard focus
- reduced motion
- canonical visual captures

Real-browser release gate: **PASS**.
