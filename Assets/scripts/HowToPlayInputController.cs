using UnityEngine;
using UnityEngine.SceneManagement;

public class HowToPlayInputController : MonoBehaviour
{
    // 次に読み込むシーン名（インスペクターからも変えられる）
    [SerializeField] private string nextSceneName = "HowToPlayScene2";

    void Update()
    {
        // Enterキー または テンキーのEnter が押されたとき
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
