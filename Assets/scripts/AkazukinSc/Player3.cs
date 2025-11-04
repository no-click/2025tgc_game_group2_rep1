using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;


[RequireComponent(typeof(Rigidbody2D))]
public class Player3 : MonoBehaviour
{
    public float speed = 5f;//動くスピード
    [SerializeField, Header("加速時のスピード倍率")]
    public float speedMultiplier = 3f;
    public GameObject bullet;//弾のプレハブ
    public GameObject item;
    public string gameOverSceneName = "GameOver";//ゲームオーバーになった際に移るシーン
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isSpeedingUp = false;
    public int hp = 2;
    public float fireRate = 0.1f;//弾の発射間隔
    private float nextFireTime;
    public bool isBarrier = false;
    [SerializeField,Header("点滅時間")]
    private float damageTime;
    [SerializeField, Header("点滅時間")]
    private float damageCycle;
    [SerializeField, Header("発射SE")]
    public AudioClip shotSE;
    public AudioClip damageSE;
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

    public void OnSpeedUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isSpeedingUp = true;
        }
        else if (context.canceled)
        {
            isSpeedingUp = false;
        }
    }

    void FixedUpdate()
    {
        float currentSpeed = speed;
        if (isSpeedingUp)
        {
            currentSpeed *= speedMultiplier;
        }
        if (isBarrier)
        {
            rb.linearVelocity = -moveInput * currentSpeed;
        }
        else
        {
            rb.linearVelocity = moveInput * currentSpeed;
        }
    }

    void Update()
    {
        int rand = Random.Range(0, 1000);
        if(item != null && rand == 0)
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
            if (items.Length > 0) return;
            float ranx = Random.Range(-8.0f, 8.0f);
            float rany = Random.Range(-4.0f, 4.0f);
            GameObject newItem = Instantiate(item, new Vector3(ranx, rany, 0.0f), Quaternion.identity);
        }
        if (Time.time >= nextFireTime)
        {
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, closestEnemy.transform.position);
                int bulletCount = GetBulletCount(distance);
                Shoot(closestEnemy, bulletCount);
                nextFireTime = Time.time + fireRate;
                if (SoundPlayer.instance != null && shotSE != null)
                {
                    SoundPlayer.instance.PlaySE(shotSE);
                }
            }
        }
        GameObject[] barriers = GameObject.FindGameObjectsWithTag("Barrier");
        isBarrier = (barriers.Length > 0);
        Damage();

    }

    //距離に応じて撃つ弾の数を返すメソッド
    public int GetBulletCount(float distance)
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
        Vector3 directionToEnemy = (target.transform.position - transform.position).normalized;
        Vector3 spawnPosition = transform.position + directionToEnemy * bulletSpawnOffset;
        float angleStep;
        if (count == 1)
        {
            angleStep = 0f;
        }
        else if (count == 2)
        {
            angleStep = 10f;
        }
        else
        {
            angleStep = 20f;
        }
        int bonusMultiplier = count;
        for (int i = 0; i < count; i++)
        {
            float currentAngle;
            if (count % 2 == 0)
            {
                currentAngle = -angleStep / 2f + (i * angleStep);
            }
            else
            {
                currentAngle = -angleStep * (count / 2) + (i * angleStep);
            }
            Vector3 bulletDirection = Quaternion.Euler(0, 0, currentAngle) * directionToEnemy;
            GameObject newBullet = Instantiate(bullet, spawnPosition, Quaternion.identity);
            Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
            Bullet3 bulletScript = newBullet.GetComponent<Bullet3>();
            if (bulletRb != null && bulletScript != null)
            {
                bulletScript.SetBonus(bonusMultiplier);
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
                CameraShaker.instance.Shake(0.3f, 0.1f);
                SoundPlayer.instance.PlaySE(damageSE);
                if (hp <= 0)
                {
                    SceneManager.LoadScene(gameOverSceneName);
                }
            }
        }else if (other.CompareTag("Item"))
        {
            hp++;
            if(hp > 10) hp = 10;
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

    public void Clear()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

}
    