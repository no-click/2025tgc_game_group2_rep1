using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class Player3 : MonoBehaviour
{
    public float speed = 5f;//動くスピード
    public GameObject bullet;//弾のプレハブ
    public string gameOverSceneName = "GameOver";//ゲームオーバーになった際に移るシーン
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public int hp = 2;
    public float fireRate = 0.1f;//弾の発射間隔
    private float nextFireTime;
    public bool isBarrier = false;
    [SerializeField,Header("点滅時間")]
    private float damageTime;
    [SerializeField, Header("点滅時間")]
    private float damageCycle;
    //点滅処理
    private SpriteRenderer spriteRenderer;
    private float damageTimeCount;
    private bool dDamage;
    //敵との距離に応じて弾の数を変えるための変数
    public float maxPowDistance = 8f; // 弾が3発になる距離
    //プレイヤーから少し離れた位置に弾を生成するための変数
    public float bulletSpawnOffset = 1.0f; // 弾の発射位置のオフセット

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        nextFireTime = Time.time;
        damageTimeCount = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        dDamage = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        if (isBarrier)
        {
            rb.linearVelocity = -moveInput * speed;
        }
        else
        {
            rb.linearVelocity = moveInput * speed;
        }
    }

    void Update()
    {

        if (Time.time >= nextFireTime)
        {
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                int bulletCount = GetBulletCount(distance);
                Shoot(closestEnemy, bulletCount);
                nextFireTime = Time.time + fireRate;
            }
        }
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        isBarrier = (barriers.Length > 0);
        Damage();

    }

    //距離に応じて撃つ弾の数を返すメソッド
    int GetBulletCount(float distance)
    {
        if (distance <= maxPowDistance)
        {
            return 3;
        }
        else if (distance <= maxPowDistance * 2)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy;
            }
        }
        return closest;
    }

    void Shoot(GameObject target, int count)
    {
        //プレイヤーから少し離れた位置を計算
        Vector3 directionToEnemy = (target.transform.position - transform.position).normalized;
        Vector3 spawnPosition = transform.position + directionToEnemy * bulletSpawnOffset;
        float angleStep;
        if (count == 1)
        {
            angleStep = 0f;
        }
        else if (count == 2)
        {
            angleStep = 10f; // 弾が2発の場合の角度差
        }
        else // count == 3
        {
            angleStep = 20f; // 弾が3発の場合の角度差
        }
        // 複数の弾を撃つためのループ
        for (int i = 0; i < count; i++)
        {
            // 角度の計算
            float currentAngle;
            if (count % 2 == 0) // 偶数個の弾の場合
            {
                currentAngle = -angleStep / 2f + (i * angleStep);
            }
            else // 奇数個の弾の場合
            {
                currentAngle = -angleStep * (count / 2) + (i * angleStep);
            }

            Vector3 bulletDirection = Quaternion.Euler(0, 0, currentAngle) * directionToEnemy;
            // 弾を計算した位置から生成して発射
            GameObject newBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            Bullet3 bulletScript = newBullet.GetComponent<Bullet3>();

            if (bulletRb != null && bulletScript != null)
            {
                bulletRb.linearVelocity = bulletDirection.normalized * bulletScript.speed;
            }
        }
    }


    void Damage()
    {
        if (!dDamage) return;
        damageTimeCount += Time.deltaTime;
        float value = Mathf.Repeat(damageTimeCount, damageCycle);
        spriteRenderer.enabled = value >= damageCycle * 0.5f;
        if(damageTimeCount >= damageTime)
        {
            damageTimeCount = 0;
            spriteRenderer.enabled = true;
            dDamage = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            if (!dDamage)
            {
                hp--;
                dDamage = true;
                if (hp <= 0)
                {
                    SceneManager.LoadScene(gameOverSceneName);
                }
            }
        }
    }

    public int GetHP()
    {
        return hp;
    }

    public bool IsDamage()
    {
        return dDamage;
    }

    public bool IsBarrier()
    {
        return isBarrier;
    }

}
    