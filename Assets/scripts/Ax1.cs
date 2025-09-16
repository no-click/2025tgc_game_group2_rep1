using UnityEngine;

public class Ax1 : MonoBehaviour
{
    private GameObject Bullet;

    void Awake()
    {
        Bullet = Resources.Load<GameObject>("Stage2/EnemyBulletEx");
        Shoot8Directions();
        Destroy(gameObject);
    }

    void Shoot8Directions()
    {
        float[] angles = { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        float ran = Random.Range(0.0f, 45.0f);

        foreach (float angle in angles)
        {
            GameObject newBullet = Instantiate(Bullet, transform.position, Quaternion.identity);
            EnemyBullet3 bulletScript = newBullet.GetComponent<EnemyBullet3>();
            if (bulletScript != null)
            {
                bulletScript.SetAngle(angle + ran);
            }
        }
    }

}