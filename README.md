# SloneUtil
Miscellaneous helpers and utilities for making games with Unity Engine.

## License
This code is freely available to you via the [WTFPL License](https://en.wikipedia.org/wiki/WTFPL)

***

## Static Classes
### SloneUtil
* **GetOrdinalString**
  * Get the ordinal string (1st, 2nd, 3rd, etc) associated with a number.
* **DistanceSquared**
  * Get the squared distance between this vector and another.
* **RotateAround**
  * Rotate a position around a specified axis and center point.
* **IsAheadOf**
  * Determine whether one object is ahead of another, according to their forward vectors and positions.
* **CapMagnitude**
  * If the length of this vector is greater than the provided maximum, cap it at that length, but maintain the direction.
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
* **InstantiateChild**
  * Instantiate one object as a child of another.
* **GetViewportSizeAtDistance**
  * Returns the world-space width and height of the screen at a specified distance in front of the camera.

### SloneUtil2D
* **GetCameraBounds**
  * Get axis-aligned bounds of the camera.
* **GetCameraSize**
  * Get the width and height (x, y) of the camera.
* **GetMouseWorldPos**
  * Get the world position of the mouse.
* **GetWorldPosFromScreen**
  **Get a position in the world from a screen position.**
* **IsPointOnscreen**
  * Returns true if the specified point is on-screen.  False if off-screen.
* **TurnToPoint**
  * Call this every frame on a transform to turn it such that the right-vector will eventually face focalPoint.
* **IsFacing**
  * Checks to see if one object is facing another in 2 dimensions.
* **RandPointInCone**
  * Generates a random point within a 2d cone.
* **RandDirection**
  * Generates a random forward vector witin a specified range.
* **GetVectorToOnscreen**
  * Get the vector of the nearest path from this point to the nearest edge of the screen.

***

## MonoBehavior Classes

### DestroyAfterTime
* Cleans up the object after a specified period of time.

### DetachFromParent
* Detaches the object from its parent immediately upon spawning.
* Allows objects to live on even if their parents will be destroyed after a time.

### WaitForButtonPress
* Will set "expired" to true when either the time expires, or one of the buttons in the "buttons" array has been pressed.
* This is intended to be created by code.  Don't add this to your object in the inspector, use GameObject.AddComponent.
* If you're confused by this, you should just use SloneUtil.WaitForButtonDown and forget you ever saw this.

***

## MoverOscillator
* Oscillates an object's position back and forth.

## MoverRotateOscillator
* Oscillates an object's rotation back and forth.

## MoverScaleOscillator
* Oscillates an object's scale back and forth.

## MoverRotator
* Rotates an object at a specified speed around each axis.

***

### GamewidePrefabs
Create a list of prefabs that should be accessible from anywhere in the game.  Create a prefab from that object, and place it in each scene where the list should be accessible.
Access the prefabs with: GameWidePrefabs.inst.GetPrefab("Prefab Name")

* **GetPrefab**
  * Retrieve a gamewide prefab from the list.

## Other Classes

### ValueOscillator
Oscillates a value back and forth at a specified time interval.

### VectorOscillator
Oscillates a set of 3 values back and fort on independent timelines.

