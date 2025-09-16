using UnityEngine;

public class Barrier : MonoBehaviour
{
    [Tooltip("弱点の方向（例: 下なら Vector2.down）")]
    public Vector2 weakDirection = Vector2.down;

    [Tooltip("弱点の許容角度（度数法）")]
    public float angleTolerance = 60f;

    [Tooltip("弱点を示す扇形Sprite")]
    public SpriteRenderer weakPointMarker;

    [Header("弱点の切替設定")]
    public float changeInterval = 10f;   // 弱点が変わる間隔（秒）
    private float timer;

    void Start()
    {
        timer = changeInterval;
    }

    void Update()
    {
        // 一定時間ごとに弱点方向をランダムに変更
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SetRandomWeakDirection();
            timer = changeInterval;
        }

        // 扇形マーカーを更新
        if (weakPointMarker != null)
        {
            float angle = Mathf.Atan2(weakDirection.y, weakDirection.x) * Mathf.Rad2Deg;
            weakPointMarker.transform.position = transform.position;
            weakPointMarker.transform.rotation = Quaternion.Euler(0, 0, angle);
            weakPointMarker.transform.localScale = new Vector3(angleTolerance / 90f, 1f, 1f);
        }
    }

    /// <summary>
    /// 攻撃が弱点方向からかどうか判定
    /// </summary>
    public bool IsAttackValid(Vector2 attackDir)
    {
        attackDir.Normalize();
        float angle = Vector2.Angle(attackDir, weakDirection);
        return angle <= angleTolerance / 2f;
    }

    /// <summary>
    /// 弱点方向をランダムに設定
    /// </summary>
    private void SetRandomWeakDirection()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        weakDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Debug.Log("新しい弱点方向: " + weakDirection);
    }
}
