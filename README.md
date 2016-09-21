# SloneUtil
Miscellaneous helpers and utilities for making games with Unity Engine.

## License
This code is freely available to you via the [WTFPL License](https://en.wikipedia.org/wiki/WTFPL)

***

## Classes
### SloneUtil (Static Class)
* **GetOrdinalString**
  * Get the ordinal string (1st, 2nd, 3rd, etc) associated with a number.
* **DistanceSquared**
  * Get the squared distance between this vector and another.
* **RotateAround**
  * Rotate a position around a specified axis and center point.
* **IsAheadOf**
  * Determine whether one object is ahead of another, according to their forward vectors and positions.
* **RandChance**
  * Returns true a specified percentage of the time.
* **DestroyAfterTime**
  * Destroys the specified gameobject after a specified period of time.
* **WaitForButtonDown**
  * Waits for button(s) press.  Has optional timeout.
* **AdvanceValue**
  * Returns a new value, advancing from the current value toward the goal at a specified rate.
  * Available for float, Vector2, Vector3 and Color.
* **AdvanceAngle**
  * Advances an angle at a specified speed, stopping once it reaches the specified angle.
* **Lerp (color)**
  * Lerp from one color to another.
* **LerpUnbounded**
  * Lerp a value, continuing beyond 100%.
  * Available for float, Vector2, and Vector3.
* **PackDate**
  * Pack a DateTime into an integer.  Accurate to within a minute.
  * Can be accurately sorted.
* **UnpackDate**
  * Unpack a DateTime which was packed with PackDate.
* **ShuffleArray**
  * Shuffle the array.  Can specify the number of times to shuffle.
* **ParseEnum**
  * Convert a string to an enum.

***

### WaitForButtonPress (MonoBehavior Class)
* Will set "expired" to false when either the time expires, or one of the buttons in the "buttons" array has been pressed.
* This is intended to be created by code.  Don't add this to your object in the inspector, use GameObject.AddComponent.
* If you're confused by this, you should just use SloneUtil.WaitForButtonDown and forget you ever saw this.

***

### GamewidePrefabs (MonoBehavior Class)
Create a list of prefabs that should be accessible from anywhere in the game.  Create a prefab from that object, and place it in each scene where the list should be accessible.
Access the prefabs with: GameWidePrefabs.inst.GetPrefab("Prefab Name")

* **GetPrefab**
  * Retrieve a gamewide prefab from the list.
