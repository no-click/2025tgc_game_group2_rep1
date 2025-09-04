using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.y <= -5.5f)
        {
            Destroy(gameObject);
        }
    }
}