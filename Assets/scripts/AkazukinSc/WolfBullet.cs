using UnityEngine;

public class WolfBullet : MonoBehaviour
{
    public float lifetime = 5.0f;

    void Start()
    {
        // �w�肳�ꂽ���Ԃ��o�߂�����ɁA���̃Q�[���I�u�W�F�N�g��j�󂷂�
        Destroy(gameObject, lifetime);
    }
}
