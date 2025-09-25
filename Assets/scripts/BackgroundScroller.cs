using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    // 移動するスピード
    public float scrollSpeed = 2f;

    // 背景がリセットされるY座標
    public float resetHeight = -10f;

    // リセット後のY座標
    public float newYPosition = 10f;

    void Update()
    {
        // 常に下に移動
        transform.Translate(Vector3.down * scrollSpeed * Time.deltaTime);

        // 指定した高さに到達したら位置をリセット
        if (transform.position.y <= resetHeight)
        {
            ResetPosition();
        }
    }

    void ResetPosition()
    {
        // Y座標をnewYPositionに、XとZ座標はそのまま
        Vector3 newPosition = new Vector3(
            transform.position.x,
            newYPosition,
            transform.position.z
        );
        transform.position = newPosition;
    }
}