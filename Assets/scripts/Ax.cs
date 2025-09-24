using UnityEngine;

public class Ax : MonoBehaviour
{
    public float speed = 5f;
    public float fireRate = 1.0f;
    public float followSpeed = 0.5f;
    public int limit = 20;
    private GameObject Bullet;
    private Rigidbody2D rb;
    private Transform player;
    private float initialAngle;
    private float nextFireTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        Bullet = Resources.Load<GameObject>("Stage2/EnemyBullet3Ex");
    }

    public void SetInitialAngle(float angle)
    {
        initialAngle = angle;
        SetDirection(initialAngle);
        nextFireTime = Time.time;
    }

    void Update()
    {
        GameObject[] enemy = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemy.Length == 0) Destroy(gameObject);
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void FixedUpdate()
    {
        // 物理演算の処理はFixedUpdateで行う
        if (player != null)
        {
            // プレイヤーへの方向ベクトルを計算
            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            // 現在の速度をプレイヤーへの方向と初期の方向の間で少しずつ調整
            Vector2 currentVelocity = rb.linearVelocity.normalized;
            Vector2 newDirection = Vector2.Lerp(currentVelocity, directionToPlayer, Time.deltaTime * followSpeed);

            // 補間された新しい方向を速度に適用
            rb.linearVelocity = newDirection * speed;

            // 斧を進行方向に向けて傾ける
            float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    void SetDirection(float angle)
    {
        float radianAngle = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));
        rb.linearVelocity = direction * speed;
    }

    void Shoot()
    {
        int bulletCount = GameObject.FindGameObjectsWithTag("EnemyBullet").Length;
        if (bulletCount <= limit)
        {
            Vector3 enemyPosition = transform.position;
            Instantiate(Bullet, enemyPosition, Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Wall"))
        {
            //if (other.gameObject.GetComponent<Player3>().IsDamage()) return;
            Destroy(gameObject);
        }
    }
}