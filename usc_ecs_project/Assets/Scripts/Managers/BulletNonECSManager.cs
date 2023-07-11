using UnityEngine;
using System.Collections.Generic;

public class BulletNonECSManager : MonoBehaviour
{
    private void Reset()
    {
        all = new List<BulletMoverNonECS>();
    }

    public static List<BulletMoverNonECS> all = new List<BulletMoverNonECS>();

    public static void Add(BulletMoverNonECS enemyNonEcs)
    {
        all.Add(enemyNonEcs);
    }

    public static void Remove(BulletMoverNonECS enemyNonEcs)
    {
        if (all.Contains(enemyNonEcs))
            all.Remove(enemyNonEcs);
    }
}