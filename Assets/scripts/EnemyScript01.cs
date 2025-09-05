using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript01 : MonoBehaviour
{

    // “®‚«‚Ì‘¬‚³
    public float speed = 5.0f;

    public string gameOverSceneName = "GameOver";


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
        else if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }

}