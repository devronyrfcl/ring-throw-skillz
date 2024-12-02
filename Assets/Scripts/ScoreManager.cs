using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int score = 0; // The player's score
    public TextMeshProUGUI[] scoreTexts; // Array to hold references to multiple TextMeshProUGUI elements

    void Start()
    {
        // Ensure that the score is displayed at the start
        UpdateScoreDisplay();
    }

    // Method to increase the score by a given amount
    public void IncreaseScore(int amount)
    {
        score += amount;
        UpdateScoreDisplay();
    }

    // Method to get the current score
    public int GetCurrentScore()
    {
        return score;
    }

    // Method to update all the UI elements showing the score
    private void UpdateScoreDisplay()
    {
        // Loop through the TextMeshProUGUI array and update each element with the current score
        foreach (TextMeshProUGUI scoreText in scoreTexts)
        {
            scoreText.text = "" + score.ToString();
        }
    }
}
