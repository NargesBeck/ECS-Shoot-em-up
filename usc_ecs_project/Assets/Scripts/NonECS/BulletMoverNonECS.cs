using System;
using UnityEngine;

public class BulletMoverNonECS : MonoBehaviour
{
    public float moveSpeed = 50f;

    private void Start()
    {
        BulletNonECSManager.Add(this);
    }

    private void Update()
    {
        MoveForward();
    }

    public void MoveForward()
    {
        transform.position += transform.forward * (Time.deltaTime * moveSpeed);
    }

    private void OnDestroy()
    {
        BulletNonECSManager.Remove(this);
    }
}