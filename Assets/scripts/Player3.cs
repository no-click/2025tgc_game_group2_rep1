using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
public class Player3 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bullet;
    public string gameOverSceneName = "GameOver";
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public int hp = 2;
    public float fireRate = 1.0f;
    private float nextFireTime;
    [SerializeField,Header("“_–ÅŽžŠÔ")]
    private float damageTime;
    [SerializeField, Header("“_–ÅŽžŠÔ")]
    private float damageCycle;

    private SpriteRenderer spriteRenderer;
    private float damageTimeCount;
    private bool dDamage;

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
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }*/
        /*int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (Time.time >= nextFireTime && enemyCount != 0)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;

        }*/
        if (Time.time >= nextFireTime)
        {
            GameObject closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                ShootAtTarget(closestEnemy);
                nextFireTime = Time.time + fireRate;
            }
        }
        Damage();

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

    void ShootAtTarget(GameObject target)
    {
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();

        if (bulletRb != null)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            Bullet3 bulletScript = newBullet.GetComponent<Bullet3>();
            if (bulletScript != null)
            {
                bulletRb.linearVelocity = direction * bulletScript.speed;
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
        if (other.CompareTag("Enemy") || other.CompareTag("OniBullet") || other.CompareTag("EnemyBullet"))
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
}
    