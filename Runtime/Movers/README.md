# Movers

Drop-on components for animating transforms without writing code. All movers extend the abstract `Mover` base class.

## Base

- [Mover.cs](Mover.cs) — Abstract base class. Provides `FindMovers()` to locate all movers on an object and optional `moverDelegate` for delegated transform control.

## Position

| Component | What it does |
|---|---|
| [MoverPositioner.cs](MoverPositioner.cs) | Base for position-based movers |
| [MoverTranslator.cs](MoverTranslator.cs) | Moves at a constant rate per axis |
| [MoveTo.cs](MoveTo.cs) | One-shot move to a target position (smooth or linear, local or world) |

## Rotation

| Component | What it does |
|---|---|
| [MoverRotator.cs](MoverRotator.cs) | Continuous rotation at a specified speed |
| [MoverRotationPositioner.cs](MoverRotationPositioner.cs) | Rotates toward a target rotation |
| [RotateTo.cs](RotateTo.cs) | One-shot rotate to target euler angles |
| [OrientByMotion2D.cs](OrientByMotion2D.cs) | Auto-orient sprite to match movement direction |

## Scale

| Component | What it does |
|---|---|
| [MoverScaler.cs](MoverScaler.cs) | Scales toward a target scale |
| [MoverScaleOscillator.cs](MoverScaleOscillator.cs) | Oscillates scale up and down |

## Oscillation

| Component | What it does |
|---|---|
| [MoverOscillator.cs](MoverOscillator.cs) | Oscillates position back and forth |
| [MoverRotateOscillator.cs](MoverRotateOscillator.cs) | Oscillates rotation back and forth |
| [ValueOscillator.cs](ValueOscillator.cs) | Oscillates a generic float value |
| [VectorOscillator.cs](VectorOscillator.cs) | Oscillates a Vector3 value |

## Effects

| Component | What it does |
|---|---|
| [Shaker.cs](Shaker.cs) | Applies random positional shake |
| [UVScroller.cs](UVScroller.cs) | Scrolls UVs on a mesh (creates a material copy) |

## Paths

| Component | What it does |
|---|---|
| [FollowObjectPath.cs](FollowObjectPath.cs) | Follows a sequence of waypoints defined by child transforms |

## Physics

| Component | What it does |
|---|---|
| [FakePhysics.cs](FakePhysics.cs) | Simplified physics without rigidbodies (velocity, drag, gravity) |
