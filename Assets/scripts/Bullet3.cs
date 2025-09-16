using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //rb.linearVelocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Witch witch = other.GetComponent<Witch>();
        if (witch != null)
        {
            Vector2 attackDir = (witch.transform.position - transform.position).normalized;
            witch.TakeDamage(1, attackDir);
            Destroy(gameObject); // íeÇÕè¡Ç¶ÇÈ
        }
        if (other.CompareTag("Enemy") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

}







