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
        // �V�����V�[����UI�e�L�X�g�ɎQ�Ƃ��㏑��
        scoreText = newScoreText;

        // �X�R�A�̌��݂̒l��V����UI�ɂ����ɕ\��
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
            // �I�u�W�F�N�g��j��
            Destroy(instance.gameObject);

            // �ÓI�ϐ���null�Ƀ��Z�b�g
            instance = null;
        }
    }

}