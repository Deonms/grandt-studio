using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))] //test code feel free to throw out
public class FirstPersonRigidbody : MonoBehaviour
{
    public float speed = 6f;
    public float mouseSensitivity = 2f;
    public Transform cam;

    private Rigidbody rb;
    private float pitch;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = true;

        if (cam == null && Camera.main != null)
            cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(0f, mouseX, 0f);
        pitch = Mathf.Clamp(pitch - mouseY, -85f, 85f);
        if (cam != null)
            cam.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = (transform.right * horizontal + transform.forward * vertical).normalized * speed;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(moveInput.x, rb.linearVelocity.y, moveInput.z);
    }
}