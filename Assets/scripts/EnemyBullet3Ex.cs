using UnityEngine;

public class EnemyBullet3Ex : MonoBehaviour
{
    [SerializeField, Header("’e‘¬")]
    public float speed = 10f;
    private Rigidbody2D rb;
    private float timer = 0f;
    private bool isMoving = false;
    private Transform playerTransform;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f && !isMoving)
        {
            if (playerTransform != null)
            {
                Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                rb.linearVelocity = directionToPlayer * speed;
                gameObject.tag = "EnemyBullet";
                isMoving = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Wall"))
        {
            if (timer < 1f) return;
            Destroy(gameObject);
        }
    }
}