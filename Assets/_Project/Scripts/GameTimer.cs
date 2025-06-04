using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float matchDuration = 60f;         // Total duration of the match in seconds
    private float timer;                      // Internal timer used to count down
    public TextMeshProUGUI timerText;         // UI text to display the timer

    private bool isGameOver = false;          // Flag to check if the game is over
    private Vector3 playerStartPosition;      // Starting position of the player
    private Quaternion playerStartRotation;   // Starting rotation of the player

    void Start()
    {
        // Set the timer to the match duration
        timer = matchDuration;
        UpdateTimerUI();

        // Find the player and save their starting position and rotation
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerStartPosition = player.transform.position;
            playerStartRotation = player.transform.rotation;
        }
    }

    void Update()
    {
        // Stop updating if the game is over
        if (isGameOver)
            return;

        // Decrease timer by deltaTime
        timer -= Time.deltaTime;
        UpdateTimerUI();

        // Check if time is up
        if (timer <= 0f)
        {
            timer = 0f;
            isGameOver = true;
            EndMatch();
        }
    }

    // Updates the UI text to show remaining time
    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timer); // Round up to the nearest whole second
            timerText.text = "Time: " + seconds;
        }
    }

    // Called when match ends
    void EndMatch()
    {
        Debug.Log("Match Ended!");
        Invoke("RestartMatch", 3f); // Wait 3 seconds before restarting the match
    }

    // Resets the timer and game-over flag
    public void ResetTimer()
    {
        timer = matchDuration;
        isGameOver = false;
    }

    // Restart the match: reset player, score, and respawn objects
    void RestartMatch()
    {
        ResetTimer();

        // Reset the score
        ScoreManager score = FindFirstObjectByType<ScoreManager>();
        if (score != null)
        {
            score.ResetScore();
        }

        // Reset player position and rotation
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.SetPositionAndRotation(playerStartPosition, playerStartRotation);

            // Reset the player's velocity if it has a Rigidbody
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = Vector3.zero;
        }

        // Respawn collectible objects or game elements
        MagnetSpawner spawner = FindFirstObjectByType<MagnetSpawner>();
        if (spawner != null)
        {
            spawner.RespawnObjects();
        }
    }
}

