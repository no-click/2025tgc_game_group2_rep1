using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneUIController : MonoBehaviour
{
    // STARTボタンから呼び出すメソッド
    public void OnClickStartButton()
    {
        // 操作説明シーンの名前
        SceneManager.LoadScene("HowToPlayScene");
    }
}
