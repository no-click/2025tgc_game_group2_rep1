using UnityEngine;
using System.Collections; // �R���[�`�����g�����߂ɕK�v�ł�
using UnityEngine.SceneManagement; // SceneManager���g�p���邽�߂ɕK�v�ł�

public class Wolf : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField, Header("�ړ��X�s�[�h")]
    public float speed = 10.0f;
    [SerializeField, Header("�ő�̗�")]
    public int hp = 100;
    [SerializeField, Header("�U���N�[���^�C��")]
    public float coolTime = 3.0f;
    public float initialDistance = 1.0f;
    [SerializeField, Header("�e�̔��ˊԊu")]
    public float fireRate = 0.5f;
    public GameObject bullet;
    private GameObject playerObject;
    private float nextActionTime;
    private Vector3 targetPosition;
    private bool isMoving = false;
    [SerializeField, Header("���S����SE")]
    public AudioClip dieSE;
    [SerializeField, Header("���̃V�[����")]
    public string nextSceneName = "GameClear";

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
        if (hp <= 0) return;
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
        currentDistance = initialDistance;
        ran = Random.Range(180.0f, 360f);
        for (int i = 0; i < 16; i++)
        {
            currentDistance *= 1.1f;
            float angle = ran - i * 22.5f;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 direction = rotation * Vector3.up;
            Vector3 spawnPosition = enemyPosition + direction * currentDistance;
            Instantiate(bullet, spawnPosition, rotation);

            yield return null;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp--;
            if (hp == 3) Time.timeScale = 0.3f;//���o
            if (hp <= 0)
            {
                // 1. ���S������̃N���[���A�b�v��SE�Đ�
                AudioSource[] audioSource = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
                if (SoundPlayer.instance != null && dieSE != null && audioSource != null)
                {
                    // ���̉������~
                    foreach (AudioSource audio in audioSource)
                    {
                        if (audio.isPlaying)
                        {
                            audio.Stop();
                        }
                    }
                    // ���SSE���Đ�
                    SoundPlayer.instance.PlaySE(dieSE);
                }

                CameraShaker.instance.Shake(1.0f, 1.0f);
                Time.timeScale = 1.0f;//���o
                GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
                foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
                // 2. �V�[���J�ڃR���[�`�����J�n
                StartCoroutine(LoadNextSceneAfterSE());

                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                gameObject.tag = "Untagged";


                // ��Destroy(gameObject); �̓V�[���J�ڌ�Ɏ����I�ɍs���邽�߁A�����ł͍폜
            }
        }
    }
}