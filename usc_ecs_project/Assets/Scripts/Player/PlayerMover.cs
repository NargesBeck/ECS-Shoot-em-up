using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    [Range(0.05f, 0.3f)] 
    [SerializeField] private float turnSpeed = 0.1f;

    private Rigidbody playerRigidbody;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    public void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    public void MovePlayer(Vector3 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0f, direction.z);
        moveDirection = moveDirection.normalized * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + moveDirection);
        RotatePlayer(moveDirection);
    }

    public void RotatePlayer(Vector3 direction)
    {
        if (direction.magnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed);
        }
    }
}