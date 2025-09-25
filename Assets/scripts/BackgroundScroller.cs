using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // �ړ�����X�s�[�h
    public float scrollSpeed = 2f;

    // �w�i�����Z�b�g�����Y���W
    public float resetHeight = -10f;

    // ���Z�b�g���Y���W
    public float newYPosition = 10f;

    void Update()
    {
        // ��ɉ��Ɉړ�
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // �w�肵�������ɓ��B������ʒu�����Z�b�g
        if (transform.position.y <= resetHeight)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // Y���W��newYPosition�ɁAX��Z���W�͂��̂܂�
        Vector3 newPosition = new Vector3(
            transform.position.x,
            newYPosition,
            transform.position.z
        );
        transform.position = newPosition;
    }
}