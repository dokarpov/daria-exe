# DARIA.EXE 3.5.0 — Gameplay Validation & Polish

## Changes

- Added a production HTTP browser-validation suite instead of relying on injected/in-memory rendering.
- Added real E2E coverage for ideal/bad Quick Play paths, replay, debrief, XP/streak/history persistence and reload.
- Added Investigation coverage for all tools, evidence gating, swipe classification, contradictions, early calls and five-move exhaustion.
- Added cross-device viewport gates at 320/360/390/430px plus landscape and short-height states.
- Added 44px touch-target, keyboard focus, reduced-motion and 200% zoom checks.
- Added service-worker registration validation on production HTTP.
- Hardened boot against corrupted localStorage history.
- Added aria-live regions for dynamic evidence/Quick Play feedback.
- Added case-content validation across the complete 24-case library.
- Removed ~97 MB of unused legacy art/audio assets from the production release.

## Release rule

`v3.5.0` should not be tagged/deployed as browser-validated until the GitHub Actions `real-browser-e2e` job passes Chromium and WebKit.