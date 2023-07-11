using System.Collections.Generic;
using UnityEngine;

public class EnemyNonECSManager : MonoBehaviour
{
    private void Reset()
    {
        all = new List<EnemyNonECS>();
    }

    public static List<EnemyNonECS> all = new List<EnemyNonECS>();

    public static void Add(EnemyNonECS enemyNonEcs)
    {
        all.Add(enemyNonEcs);
    }

    public static void Remove(EnemyNonECS enemyNonEcs)
    {
        if (all.Contains(enemyNonEcs))
            all.Remove(enemyNonEcs);
    }
}