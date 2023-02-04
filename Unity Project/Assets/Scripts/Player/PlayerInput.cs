using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private new Rigidbody rigidbody;

    private void Update()
    {
        Vector3 input = GetCameraSpaceInputDirection(Camera.main);
        MovePlayer(input);
    }

    public void MovePlayer(Vector3 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0f, direction.z);
        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;
        rigidbody.MovePosition(transform.position + moveDirection);
    }

    public Vector3 GetCameraSpaceInputDirection(Camera cam)
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(h, 0, v);

        if (cam == null)
        {
            return inputDirection;
        }

        Vector3 cameraRight = cam.transform.right;
        Vector3 cameraForward = cam.transform.forward;

        return cameraRight * inputDirection.x + cameraForward * inputDirection.z;
    }
}
