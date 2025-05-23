using UnityEngine;
using System.Collections.Generic;

public class MagnetAttractor : MonoBehaviour
{
    public float attractionForce = 10f;
    public float collectDistance = 1.5f; 
    private List<Rigidbody> objectsInRange = new List<Rigidbody>();
    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogError("ScoreManager not found in the scene!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MagnetObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && !objectsInRange.Contains(rb))
            {
                objectsInRange.Add(rb);
                Debug.Log("Object entered magnetic field: " + rb.name);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MagnetObject"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null && objectsInRange.Contains(rb))
            {
                objectsInRange.Remove(rb);
                Debug.Log("Object exited magnetic field: " + rb.name);
            }
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
                    Debug.Log("Collecting object: " + rb.name);
                    objectsInRange.RemoveAt(i);
                    Destroy(rb.gameObject);
                    if (scoreManager != null)
                    {
                        scoreManager.AddScore(1);
                    }
                }
            }
        }
    }
}
