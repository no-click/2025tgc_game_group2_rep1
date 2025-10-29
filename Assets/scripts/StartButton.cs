using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必要

public class StartButton : MonoBehaviour
{
    // 次に読み込むシーン名をインスペクタで指定
    public string nextSceneName;

    // ボタンから呼び出す関数
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(nextSceneName);
        if(nextSceneName=="Title"&& ScoreManager.instance != null) ScoreManager.ResetAndDestroy();
    }
}
