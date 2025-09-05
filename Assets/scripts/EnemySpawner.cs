using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy01;
    public GameObject Enemy02;
    public GameObject BossEnemy;

    public int level = 0;
    private int y; 
    private int x;


    // 敵がスポーンするまでのクールタイム
    public float spawnCooldown = 5.0f;
    public int firstTime = 1;
    public int spawnTime = 1;
    private float nextSpawnTime = 0f;

    void Start()
    {
        y = 5 - level;
    }

    void Update()
    {
        // 現在のシーンに存在する敵の数をカウント
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // 敵が存在せず、かつクールタイムが経過したら
        if (enemyCount == 0 && Time.time >= nextSpawnTime)
        {
            if (firstTime > 0)
            {
                Instantiate(Enemy01, new Vector3(6, 4, 0), Quaternion.identity);
                Instantiate(Enemy01, new Vector3(-6, 3, 0), Quaternion.identity);
                firstTime--;
            }else if (spawnTime > 0)
            {
                SpawnEnemy();
                spawnTime--;
            }else if(spawnTime == 0)
            {
                Instantiate(BossEnemy, new Vector3(0, 10, 0), Quaternion.identity);
                spawnTime--;
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