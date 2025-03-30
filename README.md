# FBX2SpriteSheet
FBX to Sprite Sheet Converter for Unity
 How to Use
🧍 Convert 3D FBX animation into 2D sprite frames:
Import your 3D character FBX and animation FBX into Unity.

Use your custom exporter (e.g., FBX Sprite Exporter) to:

Assign character prefab and animation

Set frame count and FPS

Export frames as PNGs (saved to Assets/CapturedFrames/)

 Generate a sprite sheet from PNG frames:
Make sure all exported PNGs are in Assets/CapturedFrames/.

Go to Tools > Generate Sprite Sheet (Grid).

This will build a sprite sheet at:
Assets/SpriteSheets/sprite_sheet_grid.png

 Automatically slice the sprite sheet:
Go to Tools > Slice Sprite Sheet (Auto).

This assumes each frame is 128x128 and arranges them in a grid.

The sliced sprites will be ready to use in your project.
Convert sprite sheet to pixel art:
Go to Tools > Downscale Sprite Sheet to Pixel Art (64x64).

This creates a new pixelated version at:
Assets/SpriteSheets/pixelart_sheet.png
