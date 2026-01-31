# Core

General-purpose static utilities and extension methods for Unity development.

## Files

### [SloneUtil.cs](SloneUtil.cs) — `CoreUtils`

Static utility class in `Slonersoft.SloneUtil.Core`. All public methods have XML doc comments for IntelliSense.

**Math & Interpolation**
- `AdvanceValue` / `AdvanceAngle` / `AdvanceEulerAngles` — Move a value toward a goal at a fixed speed per frame.
- `LerpSmooth` (float, Vector2, Vector3) — Cosine-smoothed lerp for eased departure and approach.
- `LerpUnbounded` (float, Vector2, Vector3) — Linear lerp that continues past 100%.
- `LerpEulerAngles` — Angle-aware euler lerp.
- Color `Lerp` / `AdvanceValue` — Per-channel color interpolation.

**Rotation & Direction**
- `TurnToPoint` — Per-frame turn toward a world point.
- `GetTurnToPointDelta` — Euler delta needed to face a point, with optional plane flattening.
- `RotateAround` — Rotate a point around an origin.
- `CapMagnitude` — Clamp vector magnitude.

**Camera & Viewport**
- `GetViewportSizeAtDistance` — World-space width/height at a given depth.
- `ProjectPointToNewCameraPlane` — Reproject a point to a different depth plane.
- `GetCameraPlaneProjectionScalar` — Scalar for depth-plane projection.
- `IsPointOnscreen` / `IsPointOnAnyScreen` — Viewport bounds check.

**Random**
- `RandChance` — Returns true with a given probability.
- `RandDirection` — Random normalized direction vector.

**Distance**
- `DistanceSquared` — Avoids `sqrt` for distance comparisons.
- `GetClosest` / `GetFarthest` — Nearest/farthest transform from a point.

**Data**
- `PackDate` / `UnpackDate` — Pack a `DateTime` into an int (minute precision).
- `ShuffleArray` — Fisher-Yates in-place shuffle.
- `ParseEnum` / `EnumToBitwiseFlags` — Enum utilities.
- `SnakeCaseToUppercase` / `ToRomanNumeral` — String formatting.
- `StripQuotes` — Remove surrounding double quotes from a string if present.

**Bounds**
- `BoundingBoxFromPoints` — Compute an AABB from an `IEnumerable<Vector3>`.

**GameObject**
- `InstantiateChild` — Instantiate a prefab as a child of a transform.
- `CopyNewComponent` — Clone a component onto another object via reflection.
- `MoveToCameraByReference` — Align an object to the camera by a reference transform.

**Transform Extensions**
- `ResetLocalValues` — Reset local position, rotation, and scale to identity.
- `FlattenedOnX` / `FlattenedOnY` / `FlattenedOnZ` — Zero out one axis of a Vector3.

### [GameObjectExtensions.cs](GameObjectExtensions.cs) — `SloneUtilExtensions`

- `GetOrAddComponent<T>()` — Get a component or add it if missing.
- `IsAheadOf` — Check if one transform is ahead of another by angle threshold.
- `IsFacing` — Check if a transform is facing a point.
- `DoAfterTime` — Schedule a callback after a delay.

### [SloneUtil2D.cs](SloneUtil2D.cs) — `CoreUtils2D`

2D-specific camera and input helpers: camera bounds, mouse world position, 2D facing/turning.

### [DoAfterTime.cs](DoAfterTime.cs)

MonoBehaviour that executes a callback after a specified duration, then destroys itself.

### [WaitForButtonPress.cs](WaitForButtonPress.cs)

Coroutine yield instruction that waits for a button press with optional timeout.
