using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    // �v���C���[��Transform
    public Transform playerTransform;

    // �����̑���
    public float speed = 5.0f;

    public string gameOverSceneName = "GameOver";


    void Start()
    {
        // �v���C���[�I�u�W�F�N�g���^�O�Ō���
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }
        else
        {
            Debug.LogError("Player�I�u�W�F�N�g��������܂���ł����B�v���C���[��'Player'�^�O���t���Ă��邩�m�F���Ă��������B");
        }
    }

    void Update()
    {
        // �G�̌��݈ʒu����v���C���[�̈ʒu�ցA�w�肳�ꂽ���x�ňړ�
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

        // �v���C���[�̕��������i�C�Ӂj
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