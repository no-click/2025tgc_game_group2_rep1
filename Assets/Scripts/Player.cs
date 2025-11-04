using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public GameObject bullet;
    public string gameOverSceneName = "GameOver";
    private Rigidbody2D rb;
    private Vector2 moveInput;
    public int hp = 0;
    public float fireRate = 1.0f;
    private float nextFireTime;
    [SerializeField, Header("îÌÉ_ÉÅéûÇÃSE")]
    public AudioClip damageSE;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;

        rb.freezeRotation = true;

        nextFireTime = Time.time;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = moveInput * moveSpeed;
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }*/
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (Time.time >= nextFireTime && enemyCount != 0)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
            nextFireTime = Time.time + fireRate;

        }
    }

    public int GetHP(){
        return hp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("OniBullet"))
        {
            hp--;
            CameraShaker.instance.Shake(0.3f, 0.1f);
            SoundPlayer.instance.PlaySE(damageSE);
            if (hp <= 0)
            {
                SceneManager.LoadScene(gameOverSceneName);
            }
        }
    }

}
