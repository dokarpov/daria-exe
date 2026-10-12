# DARIA.EXE — Unity Research Lab Vertical Slice

This is the first native Unity slice of the Product Office direction.

## Open
1. Open `UnityProject/` in Unity 6.
2. Let Unity import the raster assets.
3. The editor bootstrap creates/opens `Assets/Scenes/ResearchLab.unity`.
4. Press Play.

## Controls
- WASD / Arrow keys: move
- Walk into all three behaviour signals before they expire.
- R: reset collected signals

## Architecture
- Raster background: authored pixel-art room
- Foreground overlay: separate sorting layer
- Emissive/light overlay: separate sorting layer
- Player and scientist sheets: separate raster sprites
- Physics: Rigidbody2D + colliders
- Gameplay: native C#, no HTML/Canvas dependency

This is deliberately one room first. Once camera, scale, collision and layering feel right, the same structure becomes prefabs/controllers for Analytics, Roadmap, Experiment and Launch.
