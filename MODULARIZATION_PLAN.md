# SloneUtil 2.0 Modularization Plan

## Motivation

**Separate packages.** The library grew as a monolith. Projects that only need object pooling still pull in the combat system, the event framework, and all their dependencies. Splitting into per-concern UPM packages means projects take only what they use.

**Simpler namespace.** `Slonersoft.SloneUtil.Core` is redundant and verbose. The `Slonersoft` company prefix adds noise with no practical benefit for a personal library. `Slone.*` is shorter and unambiguous.

**Explicit 2D/3D distinction.** Several classes were implicitly 2D (sprite materials, 2D physics triggers, 2D orientation) with no indication in their name or namespace. In a library used across both 2D and 3D projects, this causes confusion. The new convention makes dimensionality visible at the call site.

**BlipKit removed.** BlipKit was genuinely useful for gameplay scripting and provided some systems-level value (e.g. the prefab pool lifecycle hook). However, current projects are moving toward systems-based behaviors rather than explicit scripted encounters, which is where BlipKit shines. The two broadly useful pieces (`DestroyAfterTime`, `PrefabPool`) are extracted into Assets and decoupled from the event system. BlipKit may be restored in a future version if scripted behaviors become relevant again.

**WarKit removed.** WarKit was designed for action/SHMUP/shooter games. That genre tends toward a black hole of manually-scripted encounters and level design work, which is a direction current projects are moving away from. Like BlipKit, it could be adapted for new needs later, but maintaining it now isn't worthwhile.

---

## Namespace Convention

Drop `Slonersoft.SloneUtil.*`. Use a flat two-segment scheme:

| Module | General namespace | 2D namespace |
|--------|-------------------|--------------|
| Core | `Slone.Core` | `Slone.Core2D` |
| Assets | `Slone.Assets` | — |
| Materials | `Slone.Materials` | — |
| Movers | `Slone.Movers` | — |

`2D` appended directly to the module name — valid C# identifiers, readable at a glance. Same package, separate namespace.

---

## Dependency Graph

```
Core
 ├── Assets
 ├── Materials
 └── Movers
```

All three leaf packages depend only on Core. No inter-leaf dependencies.

---

## Packages

### `com.slone.core`
**Namespaces**: `Slone.Core` / `Slone.Core2D`
**Dependencies**: none

- `CoreUtils` → `Slone.Core`
- `CoreUtils2D` → `Slone.Core2D`
- `SloneUtilExtensions` (GameObject/Transform extensions) → `Slone.Core`

**Audit needed:** check `CoreUtils` for implicitly 3D functions (e.g. Physics raycasts) and make dimensionality explicit in naming.

---

### `com.slone.assets`
**Namespace**: `Slone.Assets`
**Dependencies**: `com.slone.core`

- `ObjectPool` — keep as-is
- `PrefabPool` — moved from BlipKit, decoupled from event system
- Add `PooledObject` component — attached by `PrefabPool` to each allocated object; exposes `Free()` and a `gameObject.FreeToPool()` extension to return the object to its pool without destroying it
- `DestroyAfterTime` — moved from BlipKit; checks for `PooledObject` and calls `FreeToPool()` instead of `Destroy()` if present. No longer sends any blip events.

---

### `com.slone.materials`
**Namespace**: `Slone.Materials`
**Dependencies**: `com.slone.core`

- No changes needed

---

### `com.slone.movers`
**Namespace**: `Slone.Movers`
**Dependencies**: `com.slone.core`

- No changes needed

---

## Removed Modules

- **BlipKit** — dropped entirely
- **WarKit** — dropped entirely

---

## Migration Steps

1. **Namespace refactor** — rename all `Slonersoft.SloneUtil.*` → `Slone.*`, add `2D` suffixes, update all `using` statements. Compile and verify.
2. **Rescue survivors from BlipKit** — move `DestroyAfterTime` and `PrefabPool` into Assets; delete the rest of BlipKit.
3. **Implement `PooledObject`** — add component + `FreeToPool()` extension; update `DestroyAfterTime` and `PrefabPool` to use it instead of blip events.
4. **Delete WarKit** — remove the entire folder.
5. **Split into packages** — create per-package `package.json` and `.asmdef` files, starting from `core`, then the three leaves in any order.
6. **Retire root `package.json`** — replace with a meta-package listing all four, or remove it.
