# BlipKit

Lightweight event/message system for game object communication and lifecycle management.

## Concepts

A **Blip** is a typed event dispatched to a game object. Objects register handlers via `BlipListener`, and any script can send a blip to trigger those handlers. This decouples event producers from consumers without a global event bus.

**Event types:** `DAMAGED`, `DIED`, `DESTROYED`, `CREATED`, `ARRIVED`, `ACTIVATE`, `DEACTIVATE`, `FORCE`

## Files

### [Blip.cs](Blip.cs) — Core event system

- `Blip` — Abstract base class for event data.
- `BlipDestroy`, `BlipForce`, `BlipCreate` — Concrete event types carrying context-specific data.
- `BlipListener` — MonoBehaviour that manages handler registration and dispatch. Attach to any object that needs to send or receive blips.
- `BlipListenerUtils` — Extension methods for easy event subscription directly on GameObjects.

### Activators/

Components that send blips in response to triggers.

| Component | Trigger |
|---|---|
| [ObjActivator.cs](Activators/ObjActivator.cs) | Base class |
| [ObjActivator_Auto.cs](Activators/ObjActivator_Auto.cs) | Timer (fires once after delay) |
| [ObjActivator_Interval.cs](Activators/ObjActivator_Interval.cs) | Repeating interval |
| [ObjActivator_Trigger.cs](Activators/ObjActivator_Trigger.cs) | 3D physics trigger |
| [ObjActivator_Trigger2D.cs](Activators/ObjActivator_Trigger2D.cs) | 2D physics trigger |

### OnEvent/

Components that react to blips with gameplay actions.

| Component | Reaction |
|---|---|
| [OnEvent_SpawnObject.cs](OnEvent/OnEvent_SpawnObject.cs) | Spawn a prefab |
| [OnEvent_MoveObject.cs](OnEvent/OnEvent_MoveObject.cs) | Move an object |
| [OnEvent_ToggleObject.cs](OnEvent/OnEvent_ToggleObject.cs) | Enable/disable a GameObject |
| [OnEvent_ToggleEmitter.cs](OnEvent/OnEvent_ToggleEmitter.cs) | Toggle a particle emitter |
| [OnEvent_FlashMaterial.cs](OnEvent/OnEvent_FlashMaterial.cs) | Flash a material color |
| [InTrigger_ForceObject.cs](OnEvent/InTrigger_ForceObject.cs) | Apply force on collision |

### Lifecycle/

Components for managing object creation and destruction.

| Component | Purpose |
|---|---|
| [DestroyAfterTime.cs](Lifecycle/DestroyAfterTime.cs) | Destroy after a duration |
| [SpawnOnCreation.cs](Lifecycle/SpawnOnCreation.cs) | Spawn a prefab when created |
| [PrefabPool.cs](Lifecycle/PrefabPool.cs) | Pool manager keyed by prefab, auto-recycles on `DIED` blip |
