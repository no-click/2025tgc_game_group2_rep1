using UnityEngine;

public class EnemyScript01 : MonoBehaviour
{

    [SerializeField, Header("“®‚­‘¬‚³")]
    public float speed = 5.0f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (transform.position.x >= 7)
        {
            speed = -speed;
        }else if (transform.position.x <= -7)
        {
            speed = -speed;
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }

}