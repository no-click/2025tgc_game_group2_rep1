using UnityEngine;
using System.Collections; // コルーチンを使うために必要です

public class Wolf : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField, Header("移動スピード")]
    public float speed = 10.0f;
    [SerializeField, Header("最大体力")]
    public int hp = 100;
    [SerializeField, Header("攻撃クールタイム")]
    public float coolTime = 3.0f;
    public float initialDistance = 1.0f;
    [SerializeField, Header("弾の発射間隔")]
    public float fireRate = 0.5f;
    public GameObject bullet;
    private GameObject playerObject;
    private float nextActionTime;
    private Vector3 targetPosition;
    private bool isMoving = false;

    void Awake()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        bullet = Resources.Load<GameObject>("Stage2/EnemyBullet3EX");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
    }

    void Start()
    {
        nextActionTime = Time.time + coolTime;
    }

    void Update()
    {
        if (Time.time >= nextActionTime)
        {
            nextActionTime = Time.time + coolTime;

            if (playerTransform != null)
            {
                targetPosition = playerTransform.position;
                isMoving = true; 
            }
        }
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                speed * Time.deltaTime
            );
            if (transform.position == targetPosition)
            {
                isMoving = false;
                StartCoroutine(ShootCoroutine());
            }
        }
    }

    private IEnumerator ShootCoroutine()
    {
        float ran = Random.Range(180.0f, 360f);
        Vector3 enemyPosition = transform.position;
        float currentDistance = initialDistance;

        for (int i = 0; i < 16; i++)
        {
            currentDistance *= 1.1f;
            float angle = ran + i * 22.5f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * Vector3.up;
            Vector3 spawnPosition = enemyPosition + direction * currentDistance;
            Instantiate(bullet, spawnPosition, rotation);

            yield return null;
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