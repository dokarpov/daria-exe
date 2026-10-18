# DARIA.EXE — Unity Research Lab

This directory is an isolated Unity project. The browser build at repository root remains untouched.

## Open

Open the `unity/` directory in Unity 2022.3 LTS.

On first import the editor script creates:

- `Assets/Scenes/ResearchLab.unity`
- orthographic camera
- player movement (WASD / arrows)
- proximity interaction (E / Space)
- explicit objective / interaction HUD
- collision bounds
- four sprite render slots for background, environment, foreground occlusion, and emissive FX

You can rebuild the scene from **DARIA.EXE → Build Research Lab**.

## Art integration

The scene deliberately contains no procedural programmer-art fallback. Assign authored raster sprites to:

1. `ART_Background`
2. `ART_Environment`
3. `ART_Foreground`
4. `ART_EmissiveFX`

Player and NPC sprites are separate renderers in the next integration step.

## Current vertical-slice acceptance gate

A new player must be able to understand, without outside instructions:

1. where to go,
2. how to move,
3. when interaction is available,
4. what action succeeded,
5. what to do next.

Do not merge the Unity branch to main until the playable slice clears that gate.
