using UnityEngine;

public class EnemyNonECS : MonoBehaviour
{
    private float moveSpeed;
    private float collisionDistance = 2f;
    [SerializeField] private float health = 100;
    [SerializeField] private float damageValue = 20;

    [SerializeField] bool canHitPlayer;

    private void Update()
    {
        Vector3 playerPosition = Game.GetPlayerPosition();
        MoveForward(playerPosition);
        FacePlayer(playerPosition);
        CheckPlayerCollision(playerPosition);
        CheckBulletCollisions();
    }

    public void SetMoveSpeed(float speedValue) => moveSpeed = speedValue;

    public void CheckBulletCollisions()
    {
        GameObject[] allBullets = GameObject.FindGameObjectsWithTag("Bullet");

        for (var index = 0; index < allBullets.Length; index++)
        {
            Vector3 bullet = new Vector3(allBullets[index].transform.position.x, 0, allBullets[index].transform.position.z);
            Vector3 enemy = new Vector3(transform.position.x, 0, transform.position.z);

            if (Vector3.Distance(bullet, enemy) < collisionDistance)
            {
                health -= damageValue;
                if (health <= 0)
                {
                    Game.CreateCrystal(transform.position);
                    Destroy(allBullets[index].gameObject);
                    Destroy(gameObject);
                }
                break;
            }
        }
    }

    public void CheckPlayerCollision(Vector3 playerPosition)
    {
        playerPosition.y = transform.position.y;
        if (Vector3.Distance(playerPosition, transform.position) < collisionDistance)
        {
            Game.CreateCrystal(transform.position);
            Destroy(gameObject);

            if (canHitPlayer)
                Game.EndGame();
        }
    }

    public void MoveForward(Vector3 playerPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, Time.deltaTime * moveSpeed);
    }

    public void FacePlayer(Vector3 playerPosition)
    {
        Vector3 direction = playerPosition - transform.position;
        direction.y = 0f;

        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
    }

    private void OnDestroy()
    {
        EnemyNonECSManager.Remove(this);
    }
}