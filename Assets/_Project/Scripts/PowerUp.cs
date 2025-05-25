using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostDuration = 5f;
    public AudioClip powerUpSound; 
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MagnetAttractor magnet = other.GetComponentInChildren<MagnetAttractor>();
            if (magnet != null)
            {
                magnet.BoostMagnetForce(boostMultiplier, boostDuration);
            }

            if (powerUpSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(powerUpSound);
            }

            Destroy(gameObject, 0.2f); 
        }
    }
}




