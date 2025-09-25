using UnityEngine;

public class EnemyScript02 : MonoBehaviour
{
    // �v���C���[��Transform
    private Transform playerTransform;
    [SerializeField, Header("�ǂ��Ă���X�s�[�h")]
    public float speed = 5.0f;
    [SerializeField, Header("�ő�̗�")]
    public int hp = 1;


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