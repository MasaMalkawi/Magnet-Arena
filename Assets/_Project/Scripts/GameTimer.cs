using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float matchDuration = 60f;
    private float timer;
    public TextMeshProUGUI timerText;

    private bool isGameOver = false;

    void Start()
    {
        timer = matchDuration;
        UpdateTimerUI();
    }

    void Update()
    {
        if (isGameOver)
            return;

        timer -= Time.deltaTime;
        UpdateTimerUI();

        if (timer <= 0f)
        {
            timer = 0f;
            isGameOver = true;
            EndMatch();
        }
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timer);
            timerText.text = "Time: " + seconds;
        }
    }

    void EndMatch()
    {
        Debug.Log("Match Ended!");
        Invoke("RestartMatch", 3f);
    }

    public void ResetTimer()
    {
        timer = matchDuration;
        isGameOver = false;
    }

    void RestartMatch()
    {
        ResetTimer();

        // Reset score
        ScoreManager score = FindObjectOfType<ScoreManager>();
        if (score != null)
        {
            score.ResetScore();
        }

        // Reset player position
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0f, 1f, 0f); // Adjust as needed
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = Vector3.zero;
        }

        // Respawn magnet objects
        MagnetSpawner spawner = FindObjectOfType<MagnetSpawner>();
        if (spawner != null)
        {
            spawner.RespawnObjects();
        }
    }
}

