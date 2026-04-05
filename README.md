# SloneUtil

Miscellaneous helpers and utilities for making games with Unity Engine. Distributed as a Unity package (`com.slonersoft.slone-util`).

## License

[MIT License](LICENSE)

## Installation

In Unity, open the Package Manager, click the **+** button, and select **Install package from git URL**. Enter:

```
git@github.com:slonermike/SloneUtil.git
```

## Modules

### [Core](Runtime/Core/)

General-purpose static utilities and extension methods.

- **[CoreUtils](Runtime/Core/SloneUtil.cs)** (`Slonersoft.SloneUtil.Core`) — Math helpers (lerp, smooth lerp, advance-toward-goal for floats/vectors/colors/angles), distance/rotation utilities, camera projection, date packing, array shuffling, enum parsing, and more. All public methods have XML doc comments for IntelliSense.
- **[GameObjectExtensions](Runtime/Core/GameObjectExtensions.cs)** — Extension methods: `GetOrAddComponent<T>()`, `IsAheadOf()`, `IsFacing()`, `DoAfterTime()`.
- **[SloneUtil2D](Runtime/Core/SloneUtil2D.cs)** — 2D-specific camera and input helpers (camera bounds, mouse world position, 2D facing/turning).

### [Movers](Runtime/Movers/)

Drop-on components for animating transforms without code.

| Component | What it does |
|---|---|
| `MoverOscillator` | Oscillates position back and forth |
| `MoverRotateOscillator` | Oscillates rotation back and forth |
| `MoverScaleOscillator` | Oscillates scale back and forth |
| `MoverRotator` | Continuous rotation at a specified speed |
| `MoverScaler` | Scales over time |
| `MoverTranslator` | Moves at a constant rate per axis |
| `MoverPositioner` | Moves toward a target position |
| `MoverRotationPositioner` | Rotates toward a target rotation |
| `MoveTo` / `RotateTo` | One-shot move/rotate to a destination |
| `Shaker` | Applies random shake to position |
| `UVScroller` | Scrolls UVs on a mesh (creates material copy) |
| `OrientByMotion2D` | Orients sprite by movement direction |
| `FollowObjectPath` | Follows a sequence of waypoints |

### [Materials](Runtime/Materials/)

Runtime material manipulation without touching source assets.

- `MaterialInstancer` / `SharedMaterialInstance` / `SpriteMaterialInstance` — Safe per-object material copies.
- `MaterialColorChanger` / `MaterialValueChanger` — Animate shader properties over time.
- `MaterialFlasher` / `ShaderPulse` — Flash or pulse a shader value.

### [BlipKit](Runtime/BlipKit/)

Lightweight event/message system for game object communication.

- **[Blip](Runtime/BlipKit/Blip.cs)** — Event types: `DAMAGED`, `DIED`, `DESTROYED`, `CREATED`, `ARRIVED`, `ACTIVATE`, `DEACTIVATE`, `FORCE`.
- **Activators** — Trigger blips automatically (`ObjActivator_Auto`), on interval (`ObjActivator_Interval`), or on physics trigger (`ObjActivator_Trigger`, `ObjActivator_Trigger2D`).
- **OnEvent handlers** — React to blips by spawning objects, toggling emitters, flashing materials, moving objects, etc.
- **Lifecycle** — `DestroyAfterTime`, `SpawnOnCreation`, `PrefabPool`.

### [WarKit](Runtime/WarKit/)

Combat building blocks for health, damage, targeting, and weapons.

- `Health` / `Damageable` — HP tracking with heal rate, damage callbacks.
- `DamageOnCollision` / `DamageDelegate` — Apply damage on physics contact.
- `Weapon` / `WeaponDamager` / `Warrior` — Weapon firing and warrior state.
- `TeamAssignment` / `Targeting` / `TargetFinder` / `FaceEnemy` — Team-based target acquisition.
- `FlashOnDamage` / `ExpandOnDamage` / `SpawnOnDeath` / `DetachOnParentDeath` — Visual/lifecycle reactions.
- `DestroyBeyondBounds` / `DestroyIfStuck` — Cleanup helpers.
- `WarKitSettings` — Global configuration.

### [AssetManagement](Runtime/AssetManagement/)

- **[ObjectPool](Runtime/AssetManagement/ObjectPool.cs)** — Generic object pooling with automatic page-based expansion.
