# DARIA.EXE 3.6.0 — Consumer Game Opening

## Changes

- Rebuilt Home around one playable case instead of a SaaS-style dashboard.
- Removed Level/XP, tool thresholds, reasoning-skill meter and empty recent-case UI from the opening screen.
- Made Daria part of the opening composition.
- Added one dominant `PLAY A CASE` CTA and a quieter Investigation route.
- The visible Home case now continues into either game mode.
- Reduced copy and card density on mobile so the core action appears in the first viewport.
- Preserved the v3.5 gameplay engine and real-browser validation suite.

## Release gate

The release must pass the full Chromium/WebKit/device Playwright suite plus v3.6 Home-specific checks before merge and Pages deployment.
