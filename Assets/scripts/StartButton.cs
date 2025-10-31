using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���J�ڂɕK�v

public class StartButton : MonoBehaviour
{
    // ���ɓǂݍ��ރV�[�������C���X�y�N�^�Ŏw��
    public string nextSceneName;

    // �{�^������Ăяo���֐�
    public void OnStartButtonClicked()
    {
        Time.timeScale = 1.0f;
        if (nextSceneName=="Title"&& ScoreManager.instance != null) ScoreManager.ResetAndDestroy();
        SceneManager.LoadScene(nextSceneName);

    }
}
