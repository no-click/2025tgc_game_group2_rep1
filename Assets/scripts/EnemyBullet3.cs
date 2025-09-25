using UnityEngine;

public class EnemyBullet3 : MonoBehaviour
{
    [SerializeField, Header("�e��")]
    public float speed = 10f;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // �O������p�x���󂯎��A���̕����ɒe�𓮂���
    public void SetAngle(float angle)
    {
        // �p�x�i�x�j�����W�A���ɕϊ�
        float radianAngle = angle * Mathf.Deg2Rad;

        // ���W�A����������x�N�g�����v�Z
        Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

        // �e�ɑ��x��K�p
        rb.linearVelocity = direction * speed;
    }

    // Update���\�b�h�͕s�v�ɂȂ邽�ߍ폜�i�������Z��SetAngle�ň�x�����s�����߁j

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