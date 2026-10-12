# DARIA.EXE validation status

## Current stable browser build

**Version:** 4.1.0  
**Branch:** `main`  
**Commit:** `dee5965a12357d1fba421e864a59deb44d94950c`

## Status

The current Product Office build is the stable rollback baseline, but it does **not** currently have a passing Product Office-specific browser regression suite.

Historical browser validation exists for earlier v3.x builds, but those tests covered the previous Quick Play / Investigation product and do not validate the current office gameplay.

## Active validation tracks

- `feature/product-office-tested` — Product Office browser UX tests. Current CI status: failing; not releasable.
- `unity/research-lab` — Unity Research Lab R&D. Current Unity RC workflow status: failing at EditMode tests; WebGL build did not run.
- `unity/research-lab-testing` — isolated Unity gameplay/graphics testing.

## Promotion rule

Do not merge experimental browser or Unity work into `main` until its own relevant CI gate passes and the playable experience has been explicitly reviewed.
