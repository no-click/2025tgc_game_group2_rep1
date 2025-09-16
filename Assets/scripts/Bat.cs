using UnityEngine;

public class UpDownMover : MonoBehaviour
{
    // �ǐՂƗh��̑��x
    public float speed = 5.0f;
    // �㉺�ɗh���͈͂̍���
    public float height = 0.2f;
    public float swaySpeed = 3.0f;
    public GameObject bullet;
    public float fireRate = 1.0f;
    private GameObject playerObject;
    private Transform playerTransform;
    private float nextFireRate;
    public int hp = 50;

    // �I�u�W�F�N�g�̏����ʒu��ۑ�
    private Vector3 initialPosition;

    void Awake()
    {
        // "Player"�^�O���t�����I�u�W�F�N�g��T��
        playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
        }

        // �I�u�W�F�N�g�̏����ʒu���L�^
        initialPosition = transform.position;
        bullet = Resources.Load<GameObject>("Stage2/EnemyBullet");
    }

    void Start()
    {
        nextFireRate = fireRate;
    }

    void Update()
    {
        // �v���C���[�I�u�W�F�N�g�����݂���ꍇ�̂ݒǐՂƗh���K�p
        if (playerTransform != null)
        {
            // �v���C���[�̕����Ɍ������Ĉړ�
            Vector3 targetPosition = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);

            // sin�֐����g���Ď��ԂƂƂ��ɏ㉺�ɗh���l���v�Z
            float verticalOffset = Mathf.Sin(Time.time * swaySpeed) * height;

            // �ǐՈʒu�ɏ㉺�̗h���K�p
            Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y + verticalOffset, targetPosition.z);

            // �I�u�W�F�N�g�̐V�����ʒu��ݒ�
            transform.position = newPosition;
        }
        if(Time.time >= nextFireRate)
        {
            Shoot(playerObject);
            nextFireRate = Time.time + fireRate;
        }
    }

    void Shoot(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject newBullet = Instantiate(bullet, transform.position, rotation);
        Rigidbody2D bulletRb = newBullet.GetComponent<Rigidbody2D>();
        if (bulletRb != null)
        {
            EnemyBullet3 bulletScript = newBullet.GetComponent<EnemyBullet3>();
            if (bulletScript != null)
            {
                bulletRb.linearVelocity = direction * bulletScript.speed;
            }
        }
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