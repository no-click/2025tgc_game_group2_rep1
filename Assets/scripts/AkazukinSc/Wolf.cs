using UnityEngine;
using System.Collections; // コルーチンを使うために必要です
using UnityEngine.SceneManagement; // SceneManagerを使用するために必要です

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
    [SerializeField, Header("死亡時のSE")]
    public AudioClip dieSE;
    [SerializeField, Header("次のシーン名")]
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
        // SEの長さに相当する時間だけ待機
        if (dieSE != null)
        {
            yield return new WaitForSeconds(dieSE.length);
        }

        // 待機後、次のシーンをロード
        SceneManager.LoadScene(nextSceneName);

        // ※シーンをロードする前にこのオブジェクトを破棄しないことが重要
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp--;
            if (hp == 3) Time.timeScale = 0.3f;//演出
            if (hp <= 0)
            {
                // 1. 死亡処理後のクリーンアップとSE再生
                AudioSource[] audioSource = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
                if (SoundPlayer.instance != null && dieSE != null && audioSource != null)
                {
                    // 他の音源を停止
                    foreach (AudioSource audio in audioSource)
                    {
                        if (audio.isPlaying)
                        {
                            audio.Stop();
                        }
                    }
                    // 死亡SEを再生
                    SoundPlayer.instance.PlaySE(dieSE);
                }

                CameraShaker.instance.Shake(1.0f, 1.0f);
                Time.timeScale = 1.0f;//演出
                GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
                foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
                // 2. シーン遷移コルーチンを開始
                StartCoroutine(LoadNextSceneAfterSE());

                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                gameObject.tag = "Untagged";


                // ★Destroy(gameObject); はシーン遷移後に自動的に行われるため、ここでは削除
            }
        }
    }
}