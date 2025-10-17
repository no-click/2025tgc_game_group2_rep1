using UnityEngine;
using System.Collections; // �R���[�`�����g�����߂ɕK�v�ł�
using UnityEngine.SceneManagement; // SceneManager���g�p���邽�߂ɕK�v�ł�

public class Witch : MonoBehaviour
{
    [SerializeField, Header("�o�ꎞ�̃X�s�[�h")]
    public float speed = 20f;
    private Player3 player;
    [SerializeField, Header("���˂���e")]
    public GameObject Ax;
    [SerializeField, Header("���˂���e")]
    public GameObject bullet;
    private Rigidbody2D rb;
    [SerializeField, Header("�ő�̗�")]
    public int hp = 300;
    private GameObject playerObject;
    private Transform playerTransform;
    private float maxHP;
    [SerializeField, Header("�U���p�x")]
    public float fireRate = 3.0f;
    private float specialTimer = 0;
    private float nextFireTime;
    private float specialFireRate = 1.5f;
    private float specialNextFireTime;
    private float specialAngle = 270.0f;
    private bool isMovingIntoPosition = true;
    private bool canSpecialAttack = true;
    [SerializeField, Header("�{�X���BGM")]
    public AudioClip BGM;
    [SerializeField, Header("���S����SE")]
    public AudioClip dieSE;
    [SerializeField, Header("�K�E�Z�̌��ʉ�")]
    public AudioClip specialSE;
    [SerializeField, Header("���̃V�[����")]
    public string nextSceneName = "GameClear";


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

    void Start()
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
            SoundPlayer.instance.PlaySE(BGM);
        }
    }

    void Update()
    {
        if (hp <= 0) return;
        //  �o�ꎞ
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
            SoundPlayer.instance.PlaySE(specialSE);

        }
        int bulletCount = (int) ((Time.time - specialTimer) * 0.7); // 1��̔��˂ŉ������Ɍ���
        if( bulletCount > 12) bulletCount = 12;
        float angleStep = 360f / bulletCount;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep + specialAngle;
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            EnemyBullet3 bulletScript = newBullet.GetComponent<EnemyBullet3>();
            if (bulletScript != null)
            {
                bulletScript.SetAngle(angle);
            }
        }
        specialAngle += 13.0f;
        // ���ˊԊu�����񂾂�Z������
        specialFireRate *= 0.9f;
        if (Time.time >= nextFireTime) canSpecialAttack = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (hp < maxHP / 2 && canSpecialAttack) return;
            hp--;
            if (hp == 3) Time.timeScale = 0.3f;//���o
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
                GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
                foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
                CameraShaker.instance.Shake(1.0f, 1.0f);
                Time.timeScale = 1.0f;//���o
                // 2. �V�[���J�ڃR���[�`�����J�n
                StartCoroutine(LoadNextSceneAfterSE());

                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                gameObject.tag = "Untagged";
            }
        }
    }

    private IEnumerator LoadNextSceneAfterSE()
    {
        // SE�̒����ɑ������鎞�Ԃ����ҋ@
        if (dieSE != null)
        {
            yield return new WaitForSeconds(dieSE.length);
        }

        // �ҋ@��A���̃V�[�������[�h
        SceneManager.LoadScene(nextSceneName);

        // ���V�[�������[�h����O�ɂ��̃I�u�W�F�N�g��j�����Ȃ����Ƃ��d�v
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
