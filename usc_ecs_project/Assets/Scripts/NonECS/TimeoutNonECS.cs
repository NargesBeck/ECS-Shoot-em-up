using UnityEngine;

public class TimeoutNonECS : MonoBehaviour
{
    [SerializeField] private float lifeTime = 4f;
    private float timer;

    private void Start()
    {
        Invoke(nameof(DisableSelf), lifeTime);
    }

    private void DisableSelf() => Destroy(gameObject);
}