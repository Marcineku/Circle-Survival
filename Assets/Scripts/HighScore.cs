using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    private Text highScore;

    private void Awake()
    {
        highScore = GetComponent<Text>();
    }

    private void Start()
    {
        float highScoreValue = PlayerPrefs.GetFloat("HighScore", 0.0f);
        highScoreValue = (float)System.Math.Round(highScoreValue, 2);

        highScore.text = "High Score: " + highScoreValue.ToString("0.00") + " s";
    }
}
