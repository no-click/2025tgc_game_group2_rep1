using UnityEngine;
using UnityEngine.SceneManagement;

public class GameQuitController : MonoBehaviour
{
    [SerializeField, Header("ゲームを終了するか")]
    public bool IsEnd = false;

    void Update()
    {
        // Escキーが押されたかチェックする
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsEnd)
            {
                QuitGame();

            }
            else
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    /// <summary>
    /// ゲームを終了する処理
    /// </summary>
    void QuitGame()
    {
        // 1. Unityエディターで実行している場合
#if UNITY_EDITOR
            // エディターでの再生を停止する
            UnityEditor.EditorApplication.isPlaying = false;
        // 2. ビルドしたアプリケーションで実行している場合
#else
        // アプリケーションを終了する
        Application.Quit();
#endif

        // 注意: WebGLでビルドした場合、Application.Quit()は機能せず、
        // ユーザーがブラウザタブを閉じる必要があります。
    }
}