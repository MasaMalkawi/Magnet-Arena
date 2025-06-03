using UnityEngine;
using TMPro;
using Photon.Pun;

public class ScoreManager : MonoBehaviourPun
{
    public int score = 0;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
            return;
        }

        if (scoreText == null)
        {
            
            GameObject scoreObj = GameObject.Find("ScoreText");
            if (scoreObj != null)
                scoreText = scoreObj.GetComponent<TextMeshProUGUI>();

            if (scoreText == null)
                Debug.LogWarning("ScoreText component not assigned and not found in scene!");
        }


        UpdateScoreUI();
    }

    public void AddScore(int value)
    {
        score += value;
        
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
            
        }
        else
        {
            Debug.LogWarning("ScoreManager: scoreText is null, cannot update UI");
        }
    }
}
