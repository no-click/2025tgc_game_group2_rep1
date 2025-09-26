using UnityEngine;

public class EnemyBossScript : MonoBehaviour
{
    public GameObject OniBullet;
    [SerializeField, Header("攻撃頻度")]
    public float fireRate = 1.0f;
    [SerializeField, Header("登場時のスピード")]
    public float speed = 5.0f;
    private float distance = 2.0f;
    [SerializeField, Header("最大体力")]
    public int hp = 5;
    private float nextFireTime;
    private float startAngle = 20.0f;


    void Awake()
    {
        OniBullet = Resources.Load<GameObject>("Stage1/OniBullet");
    }

    void Start()
    {
        nextFireTime = Time.time;
    }

    void Update()
    {
        if (transform.position.y >= 2)
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
        else if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Vector3 enemyPosition = transform.position;
        float angleOffset = 10.0f;
        startAngle += angleOffset;
        for (int i = 0; i < 8; i++)
        {
            float angle = startAngle + i * 45.0f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * Vector3.up;
            Vector3 spawnPosition = enemyPosition + direction * distance;
            Instantiate(OniBullet, spawnPosition, rotation);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && transform.position.y <= 2)
        {
            hp--;
            if(hp <= 0)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("OniBullet");
                foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
                Destroy(gameObject);
            }
        }
    }

}