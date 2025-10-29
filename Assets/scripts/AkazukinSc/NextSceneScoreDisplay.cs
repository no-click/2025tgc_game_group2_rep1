using UnityEngine;
using TMPro;

public class NextSceneScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nextSceneScoreText;

    void Start()
    {
        if (ScoreManager.instance != null)
        {
            int carriedScore = ScoreManager.instance.GetTotalScore();

            if (nextSceneScoreText != null)
            {
                nextSceneScoreText.text = "SCORE: " + carriedScore.ToString("D8");
            }

            // �y�d�v�z�Q�[���N���A��̃��X�^�[�g�Ȃǂɔ����A
            // �i�������ꂽScoreManager�́A�����Ŕj�����Ă��ǂ��ꍇ������܂�
            // Destroy(ScoreManager.instance.gameObject);
        }
    }
}