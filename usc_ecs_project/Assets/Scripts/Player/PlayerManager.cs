using UnityEngine;

[RequireComponent(typeof(PlayerMover), typeof(PlayerInput), typeof(PlayerWeapon))]
public class PlayerManager : MonoBehaviour
{
    [SerializeField] private Camera sceneCamera;

    private PlayerMover playerMover;
    private PlayerWeapon playerWeapon;

    public static string playerTagName = "Player";

    public float MoveSpeed
    {
        get => playerMover.MoveSpeed;
        set => playerMover.MoveSpeed = value;
    }

    private void Awake()
    {
        playerMover = GetComponent<PlayerMover>();
        playerWeapon = GetComponent<PlayerWeapon>();
        EnablePlayer(true);
    }

    public void Setup()
    {
        playerWeapon.SetConfig();
    }

    private void FixedUpdate()
    {
        // hide the player if destroyed
        if (Game.IsGameOver())
        {
            EnablePlayer(false);
            return;
        }
        Vector3 input = PlayerInput.GetMouseInputDirection();
        playerMover.MovePlayer(input);
    }

    public static void EnablePlayer(bool state)
    {
        GameObject[] allPlayerObjects = GameObject.FindGameObjectsWithTag(playerTagName);
        foreach (GameObject go in allPlayerObjects)
        {
            go.SetActive(state);
        }
    }

    public void SetMoveSpeed(float speed)
    {
        playerMover.MoveSpeed = speed;
    }
}