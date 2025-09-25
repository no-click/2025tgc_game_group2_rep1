using UnityEngine;

public class Witch : MonoBehaviour
{
    [SerializeField, Header("“oê‚ÌƒXƒs[ƒh")]
    public float speed = 20f;
    private Player3 player;
    [SerializeField, Header("”­Ë‚·‚é’e")]
    public GameObject Ax;
    [SerializeField, Header("”­Ë‚·‚é’e")]
    public GameObject bullet;
    private Rigidbody2D rb;
    [SerializeField, Header("Å‘å‘Ì—Í")]
    public int hp = 300;
    private GameObject playerObject;
    private Transform playerTransform;
    private float maxHP;
    [SerializeField, Header("UŒ‚•p“x")]
    public float fireRate = 3.0f;
    private float specialTimer = 0;
    private float nextFireTime;
    private float specialFireRate = 1.5f;
    private float specialNextFireTime;
    private float specialAngle = 270.0f;
    private bool isMovingIntoPosition = true;
    private bool canSpecialAttack = true;
    [SerializeField, Header("€–S‚ÌSE")]
    public AudioClip dieSE;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = FindAnyObjectByType<Player3>();
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
        //  “oê
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
        if (hp <= 50) return;
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
        int bulletCount = (int) ((Time.time - specialTimer) * 0.7); // 1‰ñ‚Ì”­Ë‚Å‰½•ûŒü‚ÉŒ‚‚Â‚©
        if( bulletCount > 12) bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep + specialAngle; // ‰ñ“]‚ğ‰Á‚¦‚é
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            EnemyBullet3 bulletScript = newBullet.GetComponent<EnemyBullet3>();
            if (bulletScript != null)
            {
                bulletScript.SetAngle(angle);
            }
        }
        specialAngle += 13.0f;
        // ”­ËŠÔŠu‚ğ‚¾‚ñ‚¾‚ñ’Z‚­‚·‚é
        specialFireRate *= 0.9f;
        if (Time.time >= nextFireTime) canSpecialAttack = false;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (hp < maxHP / 2 && canSpecialAttack) return;
            hp--;
            if (hp == 3) Time.timeScale = 0.3f;//‰‰o
            if (hp <= 0)
            {
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
                CameraShaker.instance.Shake(1.0f, 1.0f);
                playerObject.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                Time.timeScale = 1.0f;//‰‰o
                player.Clear();
                Destroy(gameObject);
            }
        }
    }
    void OnDestroy()
    {
        // ƒ{ƒX‚ª”j‰ó‚³‚ê‚½‚Æ‚«‚ÉƒGƒlƒ~[ƒoƒŒƒbƒg‚ğ‚·‚×‚Äíœ
        GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

}
