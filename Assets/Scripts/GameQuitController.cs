using UnityEngine;

public class GameQuitController : MonoBehaviour
{
    void Update()
    {
        // Escキーが押されたかチェックする
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // ゲームを終了するメソッドを呼び出す
            QuitGame();
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