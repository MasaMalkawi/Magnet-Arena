using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;

public class MagnetAttractor : MonoBehaviourPun
{
    // The force applied to attract nearby objects
    public float attractionForce = 10f;
    private float originalAttractionForce;

    // The distance at which objects are collected
    public float collectDistance = 1.5f;

    // List of objects currently within the magnet's range
    private List<Rigidbody> objectsInRange = new List<Rigidbody>();

    // Reference to the ScoreManager to increase score on collection
    private ScoreManager scoreManager;

    // UI text for showing boost status and time
    public TextMeshProUGUI boostText;
    private float boostTimeRemaining = 0f;
    private bool boostActive = false;

    // Sound to play when an object is collected
    public AudioClip tickSound;
    private AudioSource audioSource;

    void Start()
    {
        // Save the original attraction force to restore it after boost ends
        originalAttractionForce = attractionForce;

        // Find the ScoreManager component if this is the local player
        if (photonView.IsMine)
        {
            scoreManager = GetComponentInChildren<ScoreManager>();
            if (scoreManager == null)
            {
                scoreManager = GetComponentInParent<ScoreManager>();
                if (scoreManager == null)
                    Debug.LogWarning("MagnetAttractor: ScoreManager not found!");
            }
        }

        // Set up the audio source for playing sounds
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        // Set initial boost text
        if (photonView.IsMine && boostText != null)
            boostText.text = "Boost: Inactive";

        // As a fallback, search the scene for a ScoreManager
        if (scoreManager == null)
        {
            scoreManager = FindAnyObjectByType<ScoreManager>();
            if (scoreManager == null)
                Debug.LogWarning("MagnetAttractor: Still couldn't find ScoreManager in scene.");
            else
                Debug.Log("MagnetAttractor: ScoreManager found via FindAnyObjectByType.");
        }
    }

    void Update()
    {
        // Only execute on the local player
        if (!photonView.IsMine) return;

        // If boost is active and time remains, update countdown
        if (boostActive && boostTimeRemaining > 0)
        {
            boostTimeRemaining -= Time.deltaTime;

            // Update boost UI text
            if (boostText != null)
                boostText.text = "Boost: " + Mathf.CeilToInt(boostTimeRemaining) + "s";
        }
        // If boost time is over, reset magnet force and UI
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
        // Only execute on the local player
        if (!photonView.IsMine) return;

        // If object has the tag "MagnetObject", add it to the list
        if (other.CompareTag("MagnetObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !objectsInRange.Contains(rb))
                objectsInRange.Add(rb);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Only execute on the local player
        if (!photonView.IsMine) return;

        // Remove object from list when it exits the trigger
        if (other.CompareTag("MagnetObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && objectsInRange.Contains(rb))
                objectsInRange.Remove(rb);
        }
    }

    void FixedUpdate()
    {
        // Only execute on the local player
        if (!photonView.IsMine) return;

        // Loop through all objects in range and attract them
        for (int i = objectsInRange.Count - 1; i >= 0; i--)
        {
            Rigidbody rb = objectsInRange[i];
            if (rb != null)
            {
                // Calculate direction and apply force
                Vector3 direction = (transform.position - rb.position).normalized;
                rb.AddForce(direction * attractionForce);

                // Check if object is within collection distance
                float distance = Vector3.Distance(transform.position, rb.position);
                if (distance < collectDistance)
                {
                    // Remove object from list
                    objectsInRange.RemoveAt(i);

                    // Destroy the object over the network if it has a PhotonView
                    PhotonView objPhotonView = rb.GetComponent<PhotonView>();
                    if (objPhotonView != null)
                        PhotonNetwork.Destroy(objPhotonView.gameObject);
                    else
                        Destroy(rb.gameObject);

                    // Increase score if ScoreManager is found
                    if (scoreManager != null)
                    {
                        scoreManager.AddScore(1);
                        Debug.Log("MagnetAttractor: Score increased by 1.");
                    }
                    else
                    {
                        Debug.LogWarning("MagnetAttractor: scoreManager is null when trying to add score!");
                    }

                    // Play collection sound
                    PlayTickSound();
                }
            }
        }
    }

    // Play the tick sound when an object is collected
    void PlayTickSound()
    {
        if (!photonView.IsMine) return;

        if (tickSound != null && audioSource != null)
            audioSource.PlayOneShot(tickSound);
    }

    // Public method to trigger a boost that multiplies attraction force for a duration
    public void BoostMagnetForce(float multiplier, float duration)
    {
        if (!photonView.IsMine) return;

        // Stop any running coroutines before starting new boost
        StopAllCoroutines();

        // Apply boost
        attractionForce = originalAttractionForce * multiplier;
        boostTimeRemaining = duration;
        boostActive = true;

        // Update boost UI text
        if (boostText != null)
            boostText.text = "Boost: " + Mathf.CeilToInt(boostTimeRemaining) + "s";
    }
}
