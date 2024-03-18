# CameraState

This class represents the state of a camera in 3D space, including its position (`x, y, z`) and rotation (yaw, pitch, roll). It provides methods to set its state from a Transform, translate its position, lerp (linearly interpolate) towards another CameraState, and update a Transform to match its state.

## MoveAxis

This class represents an axis of movement with positive and negative directions, defined by two `KeyCodes`. It can be implicitly converted to a float representing the current state of movement along the axis, with positive key pressed being `+1`, negative key pressed being `-1`, and no keys pressed being `0`.

## SimpleCameraController

This class is a simple camera controller that allows moving and rotating the camera using keyboard and mouse input. It uses `MoveAxis` instances for defining movement keys and has settings for boost, position and rotation interpolation times, and mouse sensitivity. The camera's position and rotation are updated based on input and the defined settings.

# High-Level Overview

The system provides a simple way to control a camera in 3D space using keyboard and mouse inputs. The `SimpleCameraController` component can be attached to a camera object in Unity, and it uses the `CameraState` class to track and update the camera's position and rotation. The `MoveAxis` class is used to define the keys for movement along different axes.

When the game is running, the camera can be moved using the specified keys for horizontal, vertical, and up/down movement. The camera's speed can be increased using the shift key and adjusted with the mouse scroll wheel (boost factor). The camera's rotation is controlled by moving the mouse, with the right mouse button held down (or always if `KeepCursorLocked` is true). The mouse sensitivity can be adjusted using the `MouseSensitivityCurve`, and the Y-axis can be inverted if desired.

Overall, this system provides a straightforward way to implement fly-through camera controls in Unity, suitable for exploring 3D environments or for use in a level editor.