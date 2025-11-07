using UnityEngine;

public class Bullet3 : MonoBehaviour
{
    [SerializeField, Header("�e��")]
    public float speed = 10f;
    private Rigidbody2D rb;
    private Player3 player;
    private int bonusMultiplier = 1;
    private const int HP_SCORE_MULTIPLIER = 2939;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindAnyObjectByType<Player3>();
    }

    public void SetBonus(int multiplier)
    {
        bonusMultiplier = multiplier;
    }

    void Update()
    {
        Vector2 movement = rb.linearVelocity;
        if (movement.magnitude > 0.01f)
        {
            float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (player != null && ScoreManager.instance != null)
            {
                int playerCurrentHP = player.hp;
                int baseScore = (playerCurrentHP * HP_SCORE_MULTIPLIER);
                int calculatedScore = baseScore * bonusMultiplier;
                ScoreManager.instance.AddScore(calculatedScore);
                if (player.isDamaged)
                {
                    int currentScore = ScoreManager.instance.GetTotalScore();
                    if (currentScore % 10 == 0)
                    {
                        ScoreManager.instance.AddScore(1);
                    }
                }
            }
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}