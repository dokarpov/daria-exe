#!/usr/bin/env bash
set -euo pipefail

ROOT="$(cd "$(dirname "$0")/.." && pwd)"
PROJECT="$ROOT/unity"
OUTPUT="$ROOT/webgl-release/unity-research-lab"
EXPECTED="/Applications/Unity/Hub/Editor/2022.3.62f1/Unity.app/Contents/MacOS/Unity"

if [ -x "$EXPECTED" ]; then
  UNITY="$EXPECTED"
else
  UNITY="$(find /Applications/Unity/Hub/Editor -path '*/Unity.app/Contents/MacOS/Unity' -type f 2>/dev/null | sort -V | tail -1 || true)"
fi

if [ -z "${UNITY:-}" ] || [ ! -x "$UNITY" ]; then
  echo "Unity Editor not found."
  echo "Open Unity Hub → Installs → Install Editor, include WebGL Build Support."
  echo "Preferred project version: 2022.3.62f1 LTS."
  exit 2
fi

echo "Using Unity: $UNITY"
rm -rf "$OUTPUT"

"$UNITY"   -batchmode   -quit   -projectPath "$PROJECT"   -executeMethod DariaExe.Editor.WebGLBuild.Build   -logFile -

test -f "$OUTPUT/index.html"
test -f "$OUTPUT/release.json"

echo
echo "WebGL build passed local release checks:"
echo "  $OUTPUT"

if ! git -C "$ROOT" diff --quiet -- webgl-release/unity-research-lab 2>/dev/null ||    [ -n "$(git -C "$ROOT" ls-files --others --exclude-standard webgl-release/unity-research-lab)" ]; then
  git -C "$ROOT" add -f webgl-release/unity-research-lab
  git -C "$ROOT" commit -m "Publish Unity Research Lab WebGL RC"
  git -C "$ROOT" push origin HEAD:unity/research-lab
  echo
  echo "Uploaded. GitHub Pages will publish it at:"
  echo "https://dokarpov.github.io/daria-exe/unity/"
else
  echo "Build is unchanged; nothing to publish."
fi
