using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject Enemy01;
    public GameObject Enemy02;
    public GameObject BossEnemy;
    public string gameClearSceneName = "GameClear";//�N���A���ɑJ�ڂ���V�[����
    private int y = 10; 
    private int x;


    // �G���X�|�[������܂ł̃N�[���^�C��
    public float spawnCooldown = 5.0f;
    [SerializeField, Header("��1�E�F�[�u�̉�")]
    public int firstWave = 1;
    [SerializeField, Header("��2�E�F�[�u�̉�")]
    public int secondWave = 1;
    private float nextSpawnTime = 0f;

    void Start()
    {
    }

    void Update()
    {
        // ���݂̃V�[���ɑ��݂���G�̐����J�E���g
        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        // �G�����݂����A���N�[���^�C�����o�߂�����
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