using UnityEngine;
using UnityEngine.SceneManagement; // �V�[���J�ڂɕK�v

public class StartButton : MonoBehaviour
{
    // ���ɓǂݍ��ރV�[�������C���X�y�N�^�Ŏw��
    public string nextSceneName;

    // �{�^������Ăяo���֐�
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(nextSceneName);
        if(nextSceneName=="Title"&& ScoreManager.instance != null) ScoreManager.ResetAndDestroy();
    }
}
