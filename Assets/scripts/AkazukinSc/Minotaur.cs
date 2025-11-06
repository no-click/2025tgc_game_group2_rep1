using UnityEngine;
using System.Collections; // コルーチンを使うために必要です
using UnityEngine.SceneManagement; // SceneManagerを使用するために必要です

[RequireComponent(typeof(Rigidbody2D))]
public class Minotaur : MonoBehaviour
{
    [SerializeField, Header("登場時の速さ")]
    public float speed = 20f;
    [SerializeField, Header("発射する弾")]
    public GameObject Ax;
    [SerializeField, Header("発射する弾")]
    public GameObject Ax1;
    [SerializeField, Header("追ってくる距離")]
    public float stopDistance = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField, Header("最大体力")]
    public int hp = 300;
    private GameObject playerObject;
    private Transform playerTransform;
    private float maxHP;
    [SerializeField, Header("攻撃頻度")]
    public float fireRate = 3.0f;
    private float nextFireTime;
    private bool isMovingIntoPosition = true;
    [SerializeField, Header("ボス戦のBGM")]
    public AudioClip BGM;
    [SerializeField, Header("死亡時のSE")]
    public AudioClip dieSE;
    private Player3 player;
    [SerializeField, Header("次のシーン名")]
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
        Ax1 = Resources.Load<GameObject>("Stage2/Ax1");
        maxHP = hp;
        nextFireTime = Time.time;
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
        //  登場時
        if (isMovingIntoPosition)
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
        // プレイヤーとの距離を計算
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        // 一定距離以上なら近づく
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
            int ran = Random.Range(0, 4);
            if (ran == 0) 
            {
                Shoot8Directions();
            }
            else
            {
                Shoot();
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
                Time.timeScale = 1.0f;//演出
                GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
                foreach (GameObject obj in objects)
                {
                    Destroy(obj);
                }
                StartCoroutine(LoadNextSceneAfterSE());
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                gameObject.tag = "Untagged";
            }
        }
    }

    void OnDestroy()
    {
        // ボスが破壊されたときにエネミーバレットをすべて削除
        GameObject[] objects = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }

}
