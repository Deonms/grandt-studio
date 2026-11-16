using UnityEngine;

public class CarController : MonoBehaviour
{
    public float acceleration = 25f;
    public float maxSpeed = 18f;
    public float reverseSpeed = 8f;
    public float deceleration = 20f;
    public float turnSpeed = 140f;

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float turnInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.useGravity = true;
        }
    }
    void Update()
    {
        float moveInput = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveInput = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveInput = -1f;

        turnInput = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) turnInput = 1f;

        if (moveInput > 0f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, acceleration * Time.deltaTime);
        }
        else if (moveInput < 0f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, -reverseSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 targetVelocity = transform.forward * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;

        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float directionFactor = Mathf.Sign(currentSpeed);
            float turnAmount = turnInput * turnSpeed * directionFactor * Time.fixedDeltaTime;

            Quaternion rot = rb.rotation * Quaternion.Euler(0f, turnAmount, 0f);
            rb.MoveRotation(rot);
        }
    }
}