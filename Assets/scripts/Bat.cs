using UnityEngine;

public class UpDownMover : MonoBehaviour
{
    // 追跡と揺れの速度
    public float speed = 5.0f;
    // 上下に揺れる範囲の高さ
    public float height = 0.2f;
    public float swaySpeed = 3.0f;
    public GameObject bullet;
    public float fireRate = 1.0f;
    private GameObject playerObject;
    private Transform playerTransform;
    private float nextFireRate;
    public int hp = 50;

    // オブジェクトの初期位置を保存
    private Vector3 initialPosition;

    void Awake()
    {
        // "Player"タグが付いたオブジェクトを探す
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // オブジェクトの初期位置を記録
        initialPosition = transform.position;
        bullet = Resources.Load<GameObject>("Stage2/EnemyBullet");
    }

    void Start()
    {
        nextFireRate = fireRate;
    }

    void Update()
    {
        // プレイヤーオブジェクトが存在する場合のみ追跡と揺れを適用
        if (playerTransform != null)
        {
            // プレイヤーの方向に向かって移動
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

            // sin関数を使って時間とともに上下に揺れる値を計算
            float verticalOffset = Mathf.Sin(Time.time * swaySpeed) * height;

            // 追跡位置に上下の揺れを適用
            Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y + verticalOffset, targetPosition.z);

            // オブジェクトの新しい位置を設定
            transform.position = newPosition;
        }
        if(Time.time >= nextFireRate)
        {
            Shoot(playerObject);
            nextFireRate = Time.time + fireRate;
        }
    }

    void Shoot(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject newBullet = Instantiate(bullet, transform.position, rotation);
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            EnemyBullet3 bulletScript = newBullet.GetComponent<EnemyBullet3>();
            if (bulletScript != null)
            {
                bulletRb.linearVelocity = direction * bulletScript.speed;
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