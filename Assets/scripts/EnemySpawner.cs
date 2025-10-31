using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy01;
    public GameObject Enemy02;
    public GameObject BossEnemy;
    public string gameClearSceneName = "GameClear";//クリア時に遷移するシーン名
    private int y = 10; 
    private int x;


    // 敵がスポーンするまでのクールタイム
    public float spawnCooldown = 5.0f;
    [SerializeField, Header("第1ウェーブの回数")]
    public int firstWave = 1;
    [SerializeField, Header("第2ウェーブの回数")]
    public int secondWave = 1;
    private float nextSpawnTime = 0f;

    void Start()
    {
    }

    void Update()
    {
        // 現在のシーンに存在する敵の数をカウント
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // 敵が存在せず、かつクールタイムが経過したら
        if (enemyCount == 0 && Time.time >= nextSpawnTime)
        {
            if (firstWave > 0)
            {
                Instantiate(Enemy01, new Vector3(6, 4, 0), Quaternion.identity);
                Instantiate(Enemy01, new Vector3(-6, 3, 0), Quaternion.identity);
                firstWave--;
            }else if (secondWave > 0)
            {
                SpawnEnemy();
                secondWave--;
            }else if(secondWave == 0)
            {
                Instantiate(BossEnemy, new Vector3(0, 10, 0), Quaternion.identity);
                secondWave--;
            }
            else
            {
                //SceneManager.LoadScene(gameClearSceneName);
            }
        }
    }

    void SpawnEnemy()
    {
        x = Random.Range(-10 , 10);
        Instantiate(Enemy02, new Vector3(x, y, 0), Quaternion.identity);
        x = Random.Range(-10, 10);
        Instantiate(Enemy02, new Vector3(x, y, 0), Quaternion.identity);
        nextSpawnTime = Time.time + spawnCooldown;

    }
}