using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Enemy31 : MonoBehaviour
{
    public float speed = 5f;
    public GameObject bullet;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public int hp = 10;
    public float fireRate = 1.0f;
    private GameObject playerObject;
    private Transform playerTransform;
    private float nextFireTime;
    private float maxHP;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        nextFireTime = Time.time;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        bullet = Resources.Load<GameObject>("EnemyBullet");
        maxHP = hp;
    }


    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            if (hp > maxHP / 2)
            {
                ShootAtTarget(playerObject);
            }
            else
            {
                Shoot8Directions();
            }
            nextFireTime = Time.time + fireRate;
        }
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }

    void ShootAtTarget(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject newBullet = Instantiate(bullet, transform.position, rotation);
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
        if(bulletRb != null)
        {
            EnemyBullet3 bulletScript = newBullet.GetComponent<EnemyBullet3>();
            if (bulletScript != null)
            {
                bulletRb.linearVelocity = direction * bulletScript.speed;
            }
        }
    }

    void Shoot8Directions()
    {
        float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        float ran = Random.Range(0.0f, 45.0f);

        foreach (float angle in angles)
        {
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            EnemyBullet3 bulletScript = newBullet.GetComponent<EnemyBullet3>();
            if (bulletScript != null)
            {
                bulletScript.SetAngle(angle + ran);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
