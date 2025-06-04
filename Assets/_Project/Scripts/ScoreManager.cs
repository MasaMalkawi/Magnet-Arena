using UnityEngine;
using TMPro;
using Photon.Pun;

public class ScoreManager : MonoBehaviourPun
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        // Only allow this script to run on the local player
        if (!photonView.IsMine)
        {
            enabled = false; // Disable the script if not the owner
            return;
        }

        // If the score text reference is not assigned in the Inspector
        if (scoreText == null)
        {
            // Try to find the UI object named "ScoreText" in the scene
            GameObject scoreObj = GameObject.Find("ScoreText");
            if (scoreObj != null)
                scoreText = scoreObj.GetComponent<TextMeshProUGUI>();

        }

        UpdateScoreUI();
    }

    // Add a value to the player's score
    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI(); // Refresh the score UI
    }

    // Reset the player's score to 0
    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI(); // Refresh the score UI
    }

    // Update the UI text with the current score
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // Display the score
        }
        else
        {
            Debug.LogWarning("ScoreManager: scoreText is null, cannot update UI");
        }
    }
}
