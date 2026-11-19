using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float crouchSpeed = 2f;
    [SerializeField] private float acceleration = 12f;
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 85f;

    [Header("Crouch")]
    [SerializeField] private float standHeight = 2f;
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float crouchTransitionSpeed = 10f;
    [SerializeField] private float standCameraY = 0.8f;
    [SerializeField] private float crouchCameraY = 0.4f;

    // Set by PlayerInteractor when carrying packages (1 = normal speed)
    public float SpeedMultiplier = 1f;

    private CharacterController controller;
    private Vector3 horizontalVelocity;
    private float verticalVelocity;
    private float pitch;
    private bool isCrouching;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleCrouch();
        HandleMovement();
    }

    private void HandleLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        pitch = Mathf.Clamp(pitch - mouseY, -maxLookAngle, maxLookAngle);
        cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    private void HandleCrouch()
    {
        bool wantsCrouch = Input.GetKey(KeyCode.LeftControl);

        if (wantsCrouch)
            isCrouching = true;
        else if (isCrouching && CanStandUp())
            isCrouching = false;

        float targetHeight = isCrouching ? crouchHeight : standHeight;
        float targetCamY = isCrouching ? crouchCameraY : standCameraY;

        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);

        // Keep the feet at the same spot while the height changes
        controller.center = new Vector3(0f, (controller.height - standHeight) / 2f, 0f);

        Vector3 camPos = cameraTransform.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCamY, Time.deltaTime * crouchTransitionSpeed);
        cameraTransform.localPosition = camPos;
    }

    private bool CanStandUp()
    {
        // Cast upward from the top of the current capsule to check for a ceiling
        float radius = controller.radius * 0.9f;
        Vector3 origin = transform.position + controller.center + Vector3.up * (controller.height / 2f - radius);
        float distance = standHeight - controller.height + 0.1f;

        return !Physics.SphereCast(origin, radius, Vector3.up, out _, distance,
            ~0, QueryTriggerInteraction.Ignore);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 input = Vector3.ClampMagnitude(new Vector3(x, 0f, z), 1f);
        Vector3 desiredDir = transform.TransformDirection(input);

        float targetSpeed = walkSpeed;
        if (isCrouching) targetSpeed = crouchSpeed;
        else if (Input.GetKey(KeyCode.LeftShift) && z > 0f) targetSpeed = sprintSpeed;

        Vector3 targetVelocity = desiredDir * targetSpeed * SpeedMultiplier;
        horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, acceleration * Time.deltaTime);

        // Gravity and jumping
        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f; // small downward force keeps the controller grounded

        if (Input.GetButtonDown("Jump") && controller.isGrounded && !isCrouching)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = horizontalVelocity + Vector3.up * verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }
}