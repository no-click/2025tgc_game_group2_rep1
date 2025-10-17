using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    [SerializeField, Header("�e��")]
    public float speed = 10f;
    private Rigidbody2D rb;
    private Player3 player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Player3>();
    }

    void Update()
    {
        Vector2 movement = rb.linearVelocity;
        if (movement.magnitude > 0.01f)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

}