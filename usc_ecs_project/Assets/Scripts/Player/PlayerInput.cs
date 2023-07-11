using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private static float v, h;

    public static Vector3 GetMouseInputDirection()
    {
        if (Game.Instance.gameState != GameState.Playing) return Vector3.zero;

        var joystickDir = ScreenJoystick.Instance.Direction;
        return new Vector3(joystickDir.x, 0, joystickDir.y);
    }

    // return input vector in camera space
    public static Vector3 GetCameraSpaceInputDirection(Camera cam)
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(h, 0, v);

        if (cam == null)
            return inputDirection;

        // multiply input by camera axes to convert into camera space (but still along the xz-plane)
        Vector3 cameraRight = cam.transform.right;
        Vector3 cameraForward = cam.transform.forward;

        return cameraRight * inputDirection.x + cameraForward * inputDirection.z;
    }
}