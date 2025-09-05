using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    // プレイヤーのTransform
    public Transform playerTransform;

    // 動きの速さ
    public float speed = 5.0f;

    public string gameOverSceneName = "GameOver";


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

        // プレイヤーの方を向く（任意）
        //transform.LookAt(playerTransform);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }else if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

}