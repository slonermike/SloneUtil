# WarKit

Combat building blocks for health, damage, targeting, and weapons.

## Health & Damage

| Component | Purpose |
|---|---|
| [Damageable.cs](Damageable.cs) | Abstract base for anything that can take damage |
| [Health.cs](Health.cs) | Concrete HP tracking with heal rate, damage callbacks, and invulnerability |
| [DamageDelegate.cs](DamageDelegate.cs) | Damage event type for BlipKit integration |
| [BlipDamage.cs](BlipDamage.cs) | Blip data carrying attacker reference and damage amount |
| [DamageOnCollision.cs](DamageOnCollision.cs) | Apply damage on physics contact |

## Weapons & Warriors

| Component | Purpose |
|---|---|
| [Weapon.cs](Weapon.cs) | Base weapon class with fire control and owner tracking |
| [WeaponDamager.cs](WeaponDamager.cs) | Applies damage when a weapon projectile hits a target |
| [Warrior.cs](Warrior.cs) | Combatant with PRIMARY/SECONDARY weapon slots, trigger control, and team assignment |

## Teams & Targeting

| Component | Purpose |
|---|---|
| [Team.cs](Team.cs) | Team enum and utilities |
| [TeamAssignment.cs](TeamAssignment.cs) | Marks an object with a team allegiance |
| [Targeting.cs](Targeting.cs) | Target selection and management |
| [TargetFinder.cs](TargetFinder.cs) | Finds targets by team, distance, and line of sight |
| [FaceEnemy.cs](FaceEnemy.cs) | Rotates a warrior to face its current target |

## Damage Reactions

| Component | Reaction |
|---|---|
| [FlashOnDamage.cs](FlashOnDamage.cs) | Flash material color via MaterialFlasher |
| [ExpandOnDamage.cs](ExpandOnDamage.cs) | Briefly scale up on hit |
| [SpawnOnDeath.cs](SpawnOnDeath.cs) | Spawn effects/objects on death |
| [DetachOnParentDeath.cs](DetachOnParentDeath.cs) | Detach children when parent dies |

## Cleanup

| Component | Purpose |
|---|---|
| [DestroyBeyondBounds.cs](DestroyBeyondBounds.cs) | Destroy projectiles that leave the play area |
| [DestroyIfStuck.cs](DestroyIfStuck.cs) | Destroy objects that stop moving |

## Configuration

- [WarKitSettings.cs](WarKitSettings.cs) — Global WarKit configuration.
