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

            // 【重要】ゲームクリア後のリスタートなどに備え、
            // 永続化されたScoreManagerは、ここで破棄しても良い場合があります
            // Destroy(ScoreManager.instance.gameObject);
        }
    }
}