using UnityEngine;
using TMPro;

public class NewSceneScoreConnector : MonoBehaviour
{
    private TextMeshProUGUI scoreTextComponent;

    void Awake()
    {
        scoreTextComponent = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.ConnectNewUIText(scoreTextComponent);
        }
        else
        {
            if (scoreTextComponent != null)
            {
                scoreTextComponent.text = "Error: ScoreManager not found!";
            }
        }
    }
}