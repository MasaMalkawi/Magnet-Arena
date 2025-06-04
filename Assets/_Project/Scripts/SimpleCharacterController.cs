using UnityEngine;
using Photon.Pun;

// Ensure the GameObject has a CharacterController component
[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviourPun
{
    // Movement configuration
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float rotationSpeed = 720f;
    public float gravity = -25f;

    private float verticalSpeed = 0f;
    private CharacterController controller;
    private Transform cameraTransform;

    [Header("References")]
    public GameObject playerCamera;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        // Check if this object belongs to the local player
        if (photonView.IsMine)
        {
            // Enable the local player's camera
            if (playerCamera != null)
            {
                playerCamera.SetActive(true);
                Camera cam = playerCamera.GetComponent<Camera>();
                if (cam != null)
                    cameraTransform = cam.transform;
            }
            else
            {
                Debug.LogWarning("Player camera is not assigned!");
            }
        }
        /*else
        {
            // Disable camera for remote players
            if (playerCamera != null)
                playerCamera.SetActive(false);
        }*/
    }

    private void Update()
    {
        // Only allow movement for the local player
        if (!photonView.IsMine)
            return;

        float deltaTime = Time.deltaTime;

        Vector3 moveInput = GetMovementInput();
        Vector3 move = moveInput * moveSpeed;

        
        if (controller.isGrounded)
        {
            verticalSpeed = -1f;

            
            if (Input.GetButtonDown("Jump"))
                verticalSpeed = jumpSpeed;
        }
        else
        {
            verticalSpeed += gravity * deltaTime;
        }

        move += Vector3.up * verticalSpeed;

        controller.Move(move * deltaTime);

        RotateTowards(moveInput);
    }

    // Get the movement direction based on camera orientation and input axes
    private Vector3 GetMovementInput()
    {
        if (cameraTransform == null)
            return Vector3.zero;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate forward and right directions relative to the camera
        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 desiredMoveDirection = (x * right + z * forward);

        // Normalize to prevent faster diagonal movement
        if (desiredMoveDirection.magnitude > 1f)
            desiredMoveDirection.Normalize();

        return desiredMoveDirection;
    }

    private void RotateTowards(Vector3 direction)
    {
        //  Ignore very small movment 
        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

