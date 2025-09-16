using UnityEngine;

public class Witch : MonoBehaviour
{
    public int maxHP = 100;
    private int hp;

    [Header("�o���A�ݒ�")]
    public Barrier barrier; // �����̃o���A

    void Start()
    {
        hp = maxHP;
    }

    /// <summary>
    /// �_���[�W���󂯂鏈��
    /// </summary>
    public void TakeDamage(int damage, Vector2 attackDir)
    {
        // �o���A���L���Ȃ��_����
        if (barrier != null && !barrier.IsAttackValid(attackDir))
        {
            Debug.Log("�����̃o���A�ɒe���ꂽ�I");
            return;
        }

        hp -= damage;
        Debug.Log("������HP: " + hp);

       
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") && transform.position.y <= 2)
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Player"))
        {
            //SceneManager.LoadScene(gameOverSceneName);
        }
    }
}
