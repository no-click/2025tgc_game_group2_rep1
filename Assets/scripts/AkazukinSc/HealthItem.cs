using UnityEngine;

public class HealthItem : MonoBehaviour
{
    [SerializeField, Header("è¡ñ≈éûä‘")]
    private float lifetime = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

}