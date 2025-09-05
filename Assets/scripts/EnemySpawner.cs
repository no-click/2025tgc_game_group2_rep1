using UnityEngine;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject BossEnemy;

    public int level = 0;
    private int y; 
    private int x;


    // �G���X�|�[������܂ł̃N�[���^�C��
    public float spawnCooldown = 5.0f;
    public int spawnTime = 3;
    private float nextSpawnTime = 0f;

    void Start()
    {
        y = 5 - level;
    }

    void Update()
    {
        // ���݂̃V�[���ɑ��݂���G�̐����J�E���g
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // �G�����݂����A���N�[���^�C�����o�߂�����
        if (enemyCount == 0 && Time.time >= nextSpawnTime)
        {
            if(spawnTime > 0)
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
        Instantiate(Enemy, new Vector3(x, y, 0), Quaternion.identity);
        x = Random.Range(-10, 10);
        Instantiate(Enemy, new Vector3(x, y, 0), Quaternion.identity);
        nextSpawnTime = Time.time + spawnCooldown;

    }
}