using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class Minotaur : MonoBehaviour
{
    [SerializeField, Header("�o�ꎞ�̑���")]
    public float speed = 20f;
    [SerializeField, Header("���˂���e")]
    public GameObject Ax;
    [SerializeField, Header("���˂���e")]
    public GameObject Ax1;
    [SerializeField, Header("�ǂ��Ă��鋗��")]
    public float stopDistance = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField, Header("�ő�̗�")]
    public int hp = 300;
    private GameObject playerObject;
    private Transform playerTransform;
    private float maxHP;
    [SerializeField, Header("�U���p�x")]
    public float fireRate = 3.0f;
    private float nextFireTime;
    private bool isMovingIntoPosition = true;
    [SerializeField, Header("���S����SE")]
    public AudioClip dieSE;
    private Player3 player;


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
        Ax1 = Resources.Load<GameObject>("Stage2/Ax1");
        maxHP = hp;
        nextFireTime = Time.time;
    }

    void Update()
    {
        //  �o�ꎞ
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
        // �v���C���[�Ƃ̋������v�Z
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // ��苗���ȏ�Ȃ�߂Â�
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
            if (hp == 3) Time.timeScale = 0.3f;//���o
            if (hp <= 0)
            {
                AudioSource[] audioSource = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
                if (SoundPlayer.instance != null && dieSE != null && audioSource != null)
                {
                    foreach (AudioSource audio in audioSource)
                    {
                        if (audio.isPlaying)
                        {
                            audio.Stop();
                        }
                    }
                    SoundPlayer.instance.PlaySE(dieSE);
                }
                CameraShaker.instance.Shake(1.0f, 1.0f);
                Time.timeScale = 1.0f;//���o
                player.Clear();
                Destroy(gameObject);
            }
        }
    }

    void OnDestroy()
    {
        // �{�X���j�󂳂ꂽ�Ƃ��ɃG�l�~�[�o���b�g�����ׂč폜
        GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

}
