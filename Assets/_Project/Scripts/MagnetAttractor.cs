using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MagnetAttractor : MonoBehaviour
{
    public float attractionForce = 10f;
    private float originalAttractionForce;
    public float collectDistance = 1.5f;

    private List<Rigidbody> objectsInRange = new List<Rigidbody>();
    private ScoreManager scoreManager;

    // Boost UI
    public TextMeshProUGUI boostText;
    private float boostTimeRemaining = 0f;
    private bool boostActive = false;

    void Start()
    {
        originalAttractionForce = attractionForce;
        scoreManager = FindObjectOfType<ScoreManager>();

        if (scoreManager == null)
            Debug.LogError("ScoreManager not found!");

        if (boostText != null)
            boostText.text = "Boost: Inactive";
    }

    void Update()
    {
        if (boostActive && boostTimeRemaining > 0)
        {
            boostTimeRemaining -= Time.deltaTime;
            if (boostText != null)
                boostText.text = "Boost: " + Mathf.CeilToInt(boostTimeRemaining) + "s";
        }
        else if (boostActive && boostTimeRemaining <= 0)
        {
            attractionForce = originalAttractionForce;
            boostActive = false;
            if (boostText != null)
                boostText.text = "Boost: Inactive";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MagnetObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !objectsInRange.Contains(rb))
                objectsInRange.Add(rb);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MagnetObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && objectsInRange.Contains(rb))
                objectsInRange.Remove(rb);
        }
    }

    void FixedUpdate()
    {
        for (int i = objectsInRange.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = objectsInRange[i];
            if (rb != null)
            {
                Vector3 direction = (transform.position - rb.position).normalized;
                rb.AddForce(direction * attractionForce);

                float distance = Vector3.Distance(transform.position, rb.position);
                if (distance < collectDistance)
                {
                    objectsInRange.RemoveAt(i);
                    Destroy(rb.gameObject);

                    if (scoreManager != null)
                        scoreManager.AddScore(1);
                }
            }
        }
    }

    // Call this from PowerUp
    public void BoostMagnetForce(float multiplier, float duration)
    {
        StopAllCoroutines(); // optional safeguard
        attractionForce = originalAttractionForce * multiplier;
        boostTimeRemaining = duration;
        boostActive = true;

        if (boostText != null)
            boostText.text = "Boost: " + Mathf.CeilToInt(boostTimeRemaining) + "s";
    }
}
