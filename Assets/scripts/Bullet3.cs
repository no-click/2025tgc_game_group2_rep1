using UnityEngine;

public class Bullet3 : MonoBehaviour
{
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

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

}







