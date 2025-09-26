using UnityEngine;

public class WolfBullet : MonoBehaviour
{
    public float lifetime = 5.0f;

    void Start()
    {
        // 指定された時間が経過した後に、このゲームオブジェクトを破壊する
        Destroy(gameObject, lifetime);
    }
}
