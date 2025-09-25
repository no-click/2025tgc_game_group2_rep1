using UnityEngine;

public class Witch : MonoBehaviour
{
    public float speed = 20f;
    public GameObject Ax;
    public GameObject bullet;
    private Rigidbody2D rb;
    public int hp = 300;
    private GameObject playerObject;
    private Transform playerTransform;
    private float maxHP;
    public float fireRate = 3.0f;
    private float specialTimer = 0;
    private float nextFireTime;
    private float specialFireRate = 1.5f;
    private float specialNextFireTime;
    private float specialAngle = 270.0f;
    private bool isMovingIntoPosition = true;
    private bool canSpecialAttack = true;
    public AudioClip dieSE;


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
        bullet = Resources.Load<GameObject>("Stage2/EnemyBullet");
        maxHP = hp;
        nextFireTime = Time.time;
        specialNextFireTime = Time.time;
    }

    void Update()
    {
        //  “oêŽž
        if (isMovingIntoPosition)
        {
            if (transform.position.y > 3)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
            else
            {
                isMovingIntoPosition = false;
            }
            return;
        }
        GameObject otherBullet = GameObject.FindGameObjectWithTag("EnemyBullet");
        if (Time.time >= specialNextFireTime && hp < maxHP / 2 && canSpecialAttack)
        {
            SpecialShoot();
            specialNextFireTime = Time.time + specialFireRate;
        }
        else if (otherBullet == null && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        for (int i = 0; i < 3; i++)
        {
            float ran = Random.Range(180.0f, 360f);
            GameObject newAx = Instantiate(Ax, transform.position, Quaternion.identity);
            Ax AxScript = newAx.GetComponent<Ax>();
            if (AxScript != null)
            {
                AxScript.SetInitialAngle(ran);
            }
        }
    }

    void SpecialShoot()
    {
        if (specialFireRate == 1.5) {
            nextFireTime = Time.time + 20.0f;
            specialTimer = Time.time;

        }
        int bulletCount = (int) ((Time.time - specialTimer) * 0.7); // 1‰ñ‚Ì”­ŽË‚Å‰½•ûŒü‚ÉŒ‚‚Â‚©
        if( bulletCount > 12) bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep + specialAngle; // ‰ñ“]‚ð‰Á‚¦‚é
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            EnemyBullet3 bulletScript = newBullet.GetComponent<EnemyBullet3>();
            if (bulletScript != null)
            {
                bulletScript.SetAngle(angle);
            }
        }
        specialAngle += 13.0f;
        // ”­ŽËŠÔŠu‚ð‚¾‚ñ‚¾‚ñ’Z‚­‚·‚é
        specialFireRate *= 0.9f;
        if (Time.time >= nextFireTime) canSpecialAttack = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (hp < maxHP / 2 && canSpecialAttack) return;
            hp--;
            if (hp <= 0)
            {
                GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
                foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
                AudioSource[] audioSource = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
                if (SoundPlayer.instance != null && dieSE != null && audioSource != null)
                {
                    foreach(AudioSource audio in audioSource)
                    {
                        if(audio.isPlaying)
                        {
                            audio.Stop();
                        }
                    }
                    SoundPlayer.instance.PlaySE(dieSE);
                }
                Destroy(gameObject);
            }
        }
    }
}
