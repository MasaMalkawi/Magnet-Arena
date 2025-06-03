using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Photon.Pun;

public class MagnetAttractor : MonoBehaviourPun
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

    // Sound
    public AudioClip tickSound;
    private AudioSource audioSource;

    void Start()
    {
        originalAttractionForce = attractionForce;

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
        

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        if (photonView.IsMine && boostText != null)
            boostText.text = "Boost: Inactive";

        if (scoreManager == null)
        {
            scoreManager = FindFirstObjectByType<ScoreManager>();
            if (scoreManager == null)
                Debug.LogWarning("MagnetAttractor: Still couldn't find ScoreManager in scene.");
            else
                Debug.Log("MagnetAttractor: ScoreManager found via FindObjectOfType.");
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

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
        if (!photonView.IsMine) return;

        if (other.CompareTag("MagnetObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !objectsInRange.Contains(rb))
                objectsInRange.Add(rb);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("MagnetObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && objectsInRange.Contains(rb))
                objectsInRange.Remove(rb);
        }
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

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

                    PhotonView objPhotonView = rb.GetComponent<PhotonView>();
                    if (objPhotonView != null)
                        PhotonNetwork.Destroy(objPhotonView.gameObject);
                    else
                        Destroy(rb.gameObject);

                    if (scoreManager != null)
                    {
                        scoreManager.AddScore(1);
                        Debug.Log("MagnetAttractor: Score increased by 1.");
                    }
                    else
                    {
                        Debug.LogWarning("MagnetAttractor: scoreManager is null when trying to add score!");
                    }

                    PlayTickSound();
                }
            }
        }
    }

    void PlayTickSound()
    {
        if (!photonView.IsMine) return;

        if (tickSound != null && audioSource != null)
            audioSource.PlayOneShot(tickSound);
    }

    public void BoostMagnetForce(float multiplier, float duration)
    {
        if (!photonView.IsMine) return;

        StopAllCoroutines();
        attractionForce = originalAttractionForce * multiplier;
        boostTimeRemaining = duration;
        boostActive = true;

        if (boostText != null)
            boostText.text = "Boost: " + Mathf.CeilToInt(boostTimeRemaining) + "s";
    }
}


