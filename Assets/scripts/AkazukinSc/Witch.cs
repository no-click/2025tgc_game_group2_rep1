using UnityEngine;
using System.Collections; // コルーチンを使うために必要です
using UnityEngine.SceneManagement; // SceneManagerを使用するために必要です

public class Witch : MonoBehaviour
{
    [SerializeField, Header("登場時のスピード")]
    public float speed = 20f;
    [SerializeField, Header("揺れ幅")] // ⭐ 追加
    public float swayAmplitude = 0.5f; // Y軸方向の最大移動距離

    [SerializeField, Header("揺れ速度")] // ⭐ 追加
    public float swaySpeed = 2f; // 揺れる速さ

    private Vector3 initialPosition; // ⭐ 追加: 揺れの中心位置を保持
    private Player3 player;
    [SerializeField, Header("発射する弾")]
    public GameObject Ax;
    [SerializeField, Header("発射する弾")]
    public GameObject bullet;
    private Rigidbody2D rb;
    [SerializeField, Header("最大体力")]
    public int hp = 300;
    private GameObject playerObject;
    private Transform playerTransform;
    private float maxHP;
    [SerializeField, Header("攻撃頻度")]
    public float fireRate = 3.0f;
    private float specialTimer = 0;
    private float nextFireTime;
    private float specialFireRate = 1.5f;
    private float specialNextFireTime;
    private float specialAngle = 270.0f;
    private bool isMovingIntoPosition = true;
    private bool canSpecialAttack = true;
    [SerializeField, Header("ボス戦のBGM")]
    public AudioClip BGM;
    [SerializeField, Header("死亡時のSE")]
    public AudioClip dieSE;
    [SerializeField, Header("必殺技の効果音")]
    public AudioClip specialSE;
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
        Ax = Resources.Load<GameObject>("Stage2/Ax_2");
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
        //  登場時
        if (isMovingIntoPosition)
        {
            if (transform.position.y > 3)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);
            }
            else
            {
                isMovingIntoPosition = false;
                initialPosition = transform.position;
            }
            return;
        }
        GameObject otherBullet = GameObject.FindGameObjectWithTag("EnemyBullet");
        float yOffset = Mathf.Sin(Time.time * swaySpeed) * swayAmplitude;
        transform.position = initialPosition + new Vector3(0, yOffset, 0);
        if (Time.time >= specialNextFireTime && hp < maxHP / 2 && canSpecialAttack)
        {
            SpecialShoot();
            specialNextFireTime = Time.time + specialFireRate;
            return;
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
            Ax_2 AxScript = newAx.GetComponent<Ax_2>();
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
        int bulletCount = (int) ((Time.time - specialTimer) * 0.7); // 1回の発射で何方向に撃つか
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
        // 発射間隔をだんだん短くする
        specialFireRate *= 0.9f;
        if (Time.time >= nextFireTime) canSpecialAttack = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (hp < maxHP / 2 && canSpecialAttack) return;
            hp--;
            if (hp == 3) Time.timeScale = 0.3f;//演出
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
                Time.timeScale = 1.0f;//演出
                // 2. シーン遷移コルーチンを開始
                StartCoroutine(LoadNextSceneAfterSE());

                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;
                gameObject.tag = "Untagged";
            }
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
