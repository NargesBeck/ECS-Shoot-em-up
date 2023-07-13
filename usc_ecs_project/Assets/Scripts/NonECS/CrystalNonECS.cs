using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalNonECS : MonoBehaviour
{
    float collisionDistance = 2;

    private void Update()
    {
        CheckPlayerCollision(Game.GetPlayerPosition());
    }

    public void CheckPlayerCollision(Vector3 playerPosition)
    {
        playerPosition.y = transform.position.y;
        if (Vector3.Distance(playerPosition, transform.position) < collisionDistance)
        {
            Game.CollectCrystal();
            Destroy(gameObject);
        }
    }
}
