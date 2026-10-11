# v3.5 validation status

## Required release gate
The production `dist/` must be served over HTTP and pass the Playwright suite in:
- Desktop Chromium
- Desktop WebKit
- iPhone WebKit profile
- Android Chromium profile
- iPad WebKit profile

The suite also checks reload/localStorage persistence, corrupted state recovery, service-worker registration, swipe classification, five-move exhaustion, early calls, touch targets, 320/360/390/430px layouts, landscape, short-height viewport, 200% zoom, keyboard focus, reduced motion and canonical screenshots.

## Local execution in the ChatGPT build container
The container has system Chromium, but browser navigation to HTTP/file URLs is blocked by an administrator policy (`ERR_BLOCKED_BY_ADMINISTRATOR`). WebKit binaries are not installed and external browser download is DNS-blocked. Therefore this file does **not** claim that local real-browser E2E passed.

The GitHub Actions workflow is intentionally the authoritative real-browser release gate. A release should not be tagged/deployed until `real-browser-e2e` passes.