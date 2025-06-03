/*using UnityEngine;
using Photon.Pun; 

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    public float jumpSpeed = 8f;
    public float rotationSpeed = 720f;
    public float gravity = -25f;

    private float verticalSpeed = 0f;
    private CharacterController controller;
    private Transform cameraTransform;

    private void Start()
    {
        controller = GetComponent<CharacterController>();

      
        if (photonView.IsMine)
        {
            if (Camera.main != null)
                cameraTransform = Camera.main.transform;
            else
                Debug.LogWarning("Main Camera not found! Please add a camera with the tag 'MainCamera'.");
        }
    }

    private void Update()
    {
       
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

    private Vector3 GetMovementInput()
    {
        if (cameraTransform == null)
            return Vector3.zero;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 desiredMoveDirection = (x * right + z * forward);

        if (desiredMoveDirection.magnitude > 1f)
            desiredMoveDirection.Normalize();

        return desiredMoveDirection;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}*/using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
public class SimpleCharacterController : MonoBehaviourPun
{
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

        if (photonView.IsMine)
        {
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
        else
        {
            if (playerCamera != null)
                playerCamera.SetActive(false);
        }
    }

    private void Update()
    {
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

    private Vector3 GetMovementInput()
    {
        if (cameraTransform == null)
            return Vector3.zero;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward);

        Vector3 desiredMoveDirection = (x * right + z * forward);

        if (desiredMoveDirection.magnitude > 1f)
            desiredMoveDirection.Normalize();

        return desiredMoveDirection;
    }

    private void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
