using UnityEngine;

public class Witch : MonoBehaviour
{
    public int maxHP = 100;
    private int hp;

    [Header("バリア設定")]
    public Barrier barrier; // 魔女のバリア

    void Start()
    {
        hp = maxHP;
    }

    /// <summary>
    /// ダメージを受ける処理
    /// </summary>
    public void TakeDamage(int damage, Vector2 attackDir)
    {
        // バリアが有効なら弱点判定
        if (barrier != null && !barrier.IsAttackValid(attackDir))
        {
            Debug.Log("魔女のバリアに弾かれた！");
            return;
        }

        hp -= damage;
        Debug.Log("魔女のHP: " + hp);

       
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
