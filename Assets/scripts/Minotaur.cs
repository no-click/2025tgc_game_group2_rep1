using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Minotaur : MonoBehaviour
{
    [SerializeField, Header("ìoèÍéûÇÃë¨Ç≥")]
    public float speed = 20f;
    [SerializeField, Header("î≠éÀÇ∑ÇÈíe")]
    public GameObject Ax;
    [SerializeField, Header("î≠éÀÇ∑ÇÈíe")]
    public GameObject Ax1;
    [SerializeField, Header("í«Ç¡ÇƒÇ≠ÇÈãóó£")]
    public float stopDistance = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField, Header("ç≈ëÂëÃóÕ")]
    public int hp = 300;
    private GameObject playerObject;
    private Transform playerTransform;
    private float maxHP;
    [SerializeField, Header("çUåÇïpìx")]
    public float fireRate = 3.0f;
    private float nextFireTime;
    private bool isMovingIntoPosition = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        Ax = Resources.Load<GameObject>("Stage2/Ax");
        Ax1 = Resources.Load<GameObject>("Stage2/Ax1");
        maxHP = hp;
        nextFireTime = Time.time;
    }

    void Update()
    {
        //  ìoèÍéû
        if(isMovingIntoPosition)
        {
            if (transform.position.y > 2)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
            else
            {
                isMovingIntoPosition = false;
            }
            return;
        }
        // ÉvÉåÉCÉÑÅ[Ç∆ÇÃãóó£ÇåvéZ
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // àÍíËãóó£à»è„Ç»ÇÁãﬂÇ√Ç≠
        if (distance > stopDistance)
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            float targetDistance = distance - (distance - stopDistance) / 3f;
            Vector3 targetPosition = playerTransform.position - direction * targetDistance;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        GameObject otherBullet = GameObject.FindGameObjectWithTag("EnemyBullet");
        if (otherBullet == null && Time.time >= nextFireTime)
        {
            int ran = Random.Range(0, 2);
            if (ran == 0) 
            {
                Shoot();
            }else
            {
                Shoot8Directions();
            }
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        for (int i = 0; i < 2; i++)
        {
            float ran = Random.Range(0.0f, 360f);
            GameObject newAx = Instantiate(Ax, transform.position, Quaternion.identity);
            Ax AxScript = newAx.GetComponent<Ax>();
            if (AxScript != null)
            {
                AxScript.SetInitialAngle(ran);
            }
        }
    }

    void Shoot8Directions()
    {
        for (int i = 0; i < 3; i++)
        {
            float ranx = Random.Range(-8.0f, 8.0f);
            float rany = Random.Range(-4.0f, 4.0f);
            GameObject newAx1 = Instantiate(Ax1, new Vector3(ranx, rany, 0.0f), Quaternion.identity);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp--;
            if (hp <= 0)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
                foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
                Destroy(gameObject);
            }
        }
    }

}
