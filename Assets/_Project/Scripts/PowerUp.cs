using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float boostMultiplier = 2f;
    public float boostDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MagnetAttractor magnet = other.GetComponentInChildren<MagnetAttractor>();
            if (magnet != null)
            {
                magnet.BoostMagnetForce(boostMultiplier, boostDuration);
            }

            Destroy(gameObject); 
        }
    }
}


