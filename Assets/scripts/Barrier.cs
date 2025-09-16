using UnityEngine;

public class Barrier : MonoBehaviour
{
    [Tooltip("��_�̕����i��: ���Ȃ� Vector2.down�j")]
    public Vector2 weakDirection = Vector2.down;

    [Tooltip("��_�̋��e�p�x�i�x���@�j")]
    public float angleTolerance = 60f;

    [Tooltip("��_��������`Sprite")]
    public SpriteRenderer weakPointMarker;

    [Header("��_�̐ؑ֐ݒ�")]
    public float changeInterval = 10f;   // ��_���ς��Ԋu�i�b�j
    private float timer;

    void Start()
    {
        timer = changeInterval;
    }

    void Update()
    {
        // ��莞�Ԃ��ƂɎ�_�����������_���ɕύX
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SetRandomWeakDirection();
            timer = changeInterval;
        }

        // ��`�}�[�J�[���X�V
        if (weakPointMarker != null)
        {
            float angle = Mathf.Atan2(weakDirection.y, weakDirection.x) * Mathf.Rad2Deg;
            weakPointMarker.transform.position = transform.position;
            weakPointMarker.transform.rotation = Quaternion.Euler(0, 0, angle);
            weakPointMarker.transform.localScale = new Vector3(angleTolerance / 90f, 1f, 1f);
        }
    }

    /// <summary>
    /// �U������_�������炩�ǂ�������
    /// </summary>
    public bool IsAttackValid(Vector2 attackDir)
    {
        attackDir.Normalize();
        float angle = Vector2.Angle(attackDir, weakDirection);
        return angle <= angleTolerance / 2f;
    }

    /// <summary>
    /// ��_�����������_���ɐݒ�
    /// </summary>
    private void SetRandomWeakDirection()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        weakDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Debug.Log("�V������_����: " + weakDirection);
    }
}
