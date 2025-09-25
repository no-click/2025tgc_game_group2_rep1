using UnityEngine;

public class EnemyScript02 : MonoBehaviour
{
    // プレイヤーのTransform
    private Transform playerTransform;
    [SerializeField, Header("追ってくるスピード")]
    public float speed = 5.0f;
    [SerializeField, Header("最大体力")]
    public int hp = 1;


    void Start()
    {
        // プレイヤーオブジェクトをタグで検索
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Playerオブジェクトが見つかりませんでした。プレイヤーに'Player'タグが付いているか確認してください。");
        }
    }

    void Update()
    {
        // 敵の現在位置からプレイヤーの位置へ、指定された速度で移動
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}