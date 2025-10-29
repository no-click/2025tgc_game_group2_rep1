using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int totalScore = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
            return;
        }
        UpdateScoreDisplay();
    }

    public void AddScore(int points)
    {
        totalScore += points;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + totalScore.ToString("D8");
        }
    }

    public void ConnectNewUIText(TextMeshProUGUI newScoreText)
    {
        // 新しいシーンのUIテキストに参照を上書き
        scoreText = newScoreText;

        // スコアの現在の値を新しいUIにすぐに表示
        UpdateScoreDisplay();
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public static void ResetAndDestroy()
    {
        if (instance != null)
        {
            // オブジェクトを破棄
            Destroy(instance.gameObject);

            // 静的変数もnullにリセット
            instance = null;
        }
    }

}