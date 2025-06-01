using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float matchDuration = 60f;
    private float timer;
    public TextMeshProUGUI timerText;

    private bool isGameOver = false;
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    void Start()
    {
        timer = matchDuration;
        UpdateTimerUI();

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerStartPosition = player.transform.position;
            playerStartRotation = player.transform.rotation;
        }
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

        ScoreManager score = FindFirstObjectByType<ScoreManager>();
        if (score != null)
        {
            score.ResetScore();
        }

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.SetPositionAndRotation(playerStartPosition, playerStartRotation);
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null)
                rb.linearVelocity = Vector3.zero;
        }

        MagnetSpawner spawner = FindFirstObjectByType<MagnetSpawner>();
        if (spawner != null)
        {
            spawner.RespawnObjects();
        }
    }
}
