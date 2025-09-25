using UnityEngine;

public class EnemyBullet3 : MonoBehaviour
{
    [SerializeField, Header("弾速")]
    public float speed = 10f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 外部から角度を受け取り、その方向に弾を動かす
    public void SetAngle(float angle)
    {
        // 角度（度）をラジアンに変換
        float radianAngle = angle * Mathf.Deg2Rad;

        // ラジアンから方向ベクトルを計算
        Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

        // 弾に速度を適用
        rb.linearVelocity = direction * speed;
    }

    // Updateメソッドは不要になるため削除（物理演算はSetAngleで一度だけ行うため）

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Wall"))
        {
            //if (other.gameObject.GetComponent<Player3>().IsDamage()) return;
            Destroy(gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Barrier"))
        {
            Destroy(gameObject);
        }
    }

}