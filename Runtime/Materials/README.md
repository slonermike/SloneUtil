# Materials

Runtime material manipulation without modifying source assets.

## Instancing

These components create per-object material copies so changes don't bleed across objects sharing the same material.

| Component | Use case |
|---|---|
| [MaterialInstancer.cs](MaterialInstancer.cs) | General-purpose material instance on a Renderer |
| [SharedMaterialInstance.cs](SharedMaterialInstance.cs) | Shared instance across multiple renderers on one object |
| [SpriteMaterialInstance.cs](SpriteMaterialInstance.cs) | Material instance for SpriteRenderers |

## Animation

| Component | What it does |
|---|---|
| [MaterialColorChanger.cs](MaterialColorChanger.cs) | Smoothly lerps a material color property over time |
| [MaterialValueChanger.cs](MaterialValueChanger.cs) | Smoothly lerps a float shader property over time |

## Effects

| Component | What it does |
|---|---|
| [MaterialFlasher.cs](MaterialFlasher.cs) | Composites multiple overlapping color flashes into a `_FlashColor` shader property |
| [ShaderPulse.cs](ShaderPulse.cs) | Periodically pulses a shader property value |
