using UnityEngine;
using TMPro;

public class NextSceneScoreDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nextSceneScoreText;
    private string result = "";

    void Start()
    {
        if (ScoreManager.instance != null)
        {
            int carriedScore = ScoreManager.instance.GetTotalScore();
            if (carriedScore > 70000000)
            {
                if (carriedScore % 10 == 0)
                {
                    result = "SS";

                }
                else
                {
                    result = "S";
                }
            }
            else if (carriedScore > 60000000)
            {
                result = "A";
            }
            else if (carriedScore > 50000000)
            {
                result = "B";
            }
            else if (carriedScore > 40000000)
            {
                result = "C";
            }
            else
            {
                result = "D";
            }
            if (nextSceneScoreText != null)
            {
                nextSceneScoreText.text = "SCORE: " + carriedScore.ToString("D8") + "\nRANK : " + result;
            }
        }
    }
}