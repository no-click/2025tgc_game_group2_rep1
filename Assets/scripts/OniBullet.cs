using UnityEngine;
using UnityEngine.SceneManagement;

public class OniBullet : MonoBehaviour
{
    public float moveSpeed = 5.0f;

    public float rotationSpeed = 100.0f;

    private Transform enemyTransform;

    public string gameOverSceneName = "GameOver";

    void Start()
    {
        GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
        if (enemyObject != null)
        {
            enemyTransform = enemyObject.transform;
        }

        if (enemyTransform != null)
        {
            Vector3 direction = (transform.position - enemyTransform.position).normalized;

            GetComponent<Rigidbody2D>().linearVelocity = direction * moveSpeed;
        }
    }

    void Update()
    {
        Vector3 currentDirection = GetComponent<Rigidbody2D>().linearVelocity;
        Vector3 rotatedDirection = Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime) * currentDirection;

        GetComponent<Rigidbody2D>().linearVelocity = rotatedDirection.normalized * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            //SceneManager.LoadScene(gameOverSceneName);
        }
    }

}