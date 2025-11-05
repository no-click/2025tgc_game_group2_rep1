using UnityEngine;
using TMPro;

public class NextSceneScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nextSceneScoreText;
    private string result = "";
    public VideoController VideoController;

    void Start()
    {
        VideoController = FindAnyObjectByType<VideoController>();
        if (ScoreManager.instance != null)
        {
            int carriedScore = ScoreManager.instance.GetTotalScore();
            if (carriedScore > 100000000 && carriedScore % 10 == 0)
            {
                result = "SS";
                VideoController.PlayVideo();
            }
            else if (carriedScore > 90000000)
            {
                result = "S";
            }
            else if (carriedScore > 80000000)
            {
                result = "A";
            }
            else if (carriedScore > 70000000)
            {
                result = "B";
            }
            else
            {
                result = "C";
            }
            if (nextSceneScoreText != null)
            {
                nextSceneScoreText.text = "SCORE: " + carriedScore.ToString("D8") + "\nRANK : " + result;
            }
        }else
        {
            VideoController.PlayVideo();
        }
    }
}