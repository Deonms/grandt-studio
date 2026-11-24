using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CarController : MonoBehaviour
{
    public float acceleration = 25f;
    public float maxSpeed = 18f;
    public float reverseSpeed = 8f;
    public float deceleration = 20f;
    public float turnSpeed = 140f;
    [Header("Drift Settings")]
    public float driftTurnMultiplier = 1.5f;
    public float driftGrip = 0.95f;
    [Header("Engine Audio Settings")]
    public AudioClip engineClip;
    public float minPitch = 0.5f;
    public float maxPitch = 1.30f;
    public float minVolume = 0.01f;
    public float maxVolume = 0.05f;
    public float audioSmoothSpeed = 8f;
    [Header("Reverse Audio Settings")]
    public AudioClip reverseClip;
    [Range(0f, 1f)]
    public float reverseVolume = 1f;
    public float reverseFadeSpeed = 10f;
    public bool requireReverseMovement = true;
    private Rigidbody rb;
    private AudioSource engineAudio;
    private AudioSource reverseAudio;
    private float currentSpeed = 0f;
    private float turnInput = 0f;
    private bool isDrifting = false;
    private float moveInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.useGravity = true;
        }

        engineAudio = GetComponent<AudioSource>();
        if (engineClip != null)
        {
            engineAudio.clip = engineClip;
        }
        engineAudio.loop = true;

        if (engineAudio.clip != null && !engineAudio.isPlaying)
        {
            engineAudio.Play();
        }
        reverseAudio = gameObject.AddComponent<AudioSource>();
        reverseAudio.clip = reverseClip;
        reverseAudio.loop = true;
        reverseAudio.playOnAwake = false;
        reverseAudio.volume = 0f;
    }
    void Update()
    {
        moveInput = 0f;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) moveInput = 1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) moveInput = -1f;
        turnInput = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) turnInput = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) turnInput = 1f;

        isDrifting = Input.GetKey(KeyCode.LeftShift);

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
        UpdateEngineAudio();
        UpdateReverseAudio();
    }

    void UpdateEngineAudio()
    {
        if (engineAudio == null || engineAudio.clip == null) return;

        float speedRatio = Mathf.Clamp01(Mathf.Abs(currentSpeed) / maxSpeed);

        float targetPitch = Mathf.Lerp(minPitch, maxPitch, speedRatio);
        float targetVolume = Mathf.Lerp(minVolume, maxVolume, speedRatio);

        bool shouldPlayReverse = requireReverseMovement
            ? currentSpeed < -0.05f
            : moveInput < 0f;
        if (shouldPlayReverse && reverseAudio != null && reverseAudio.clip != null)
        {
            targetVolume = 0f;
        }

        engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, targetPitch, audioSmoothSpeed * Time.deltaTime);
        engineAudio.volume = Mathf.Lerp(engineAudio.volume, targetVolume, audioSmoothSpeed * Time.deltaTime);
    }

    void UpdateReverseAudio()
    {
        if (reverseAudio == null || reverseAudio.clip == null) return;

        bool shouldPlayReverse = requireReverseMovement
            ? currentSpeed < -0.05f
            : moveInput < 0f;

        if (shouldPlayReverse && !reverseAudio.isPlaying)
        {
            reverseAudio.Play();
        }
        float targetVolume = shouldPlayReverse ? Mathf.Clamp01(reverseVolume) : 0f;
        reverseAudio.volume = Mathf.MoveTowards(reverseAudio.volume, targetVolume, reverseFadeSpeed * Time.deltaTime);

        if (!shouldPlayReverse && reverseAudio.volume <= 0.001f && reverseAudio.isPlaying)
        {
            reverseAudio.Stop();
        }
    }
    void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 forwardVelocity = transform.forward * currentSpeed;
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (isDrifting)
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, forwardVelocity, driftGrip * Time.fixedDeltaTime);
        }
        else
        {
            horizontalVelocity = forwardVelocity;
        }
        horizontalVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = horizontalVelocity;
        if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            float directionFactor = Mathf.Sign(currentSpeed);
            float currentTurnSpeed = turnSpeed;

            if (isDrifting)
            {
                currentTurnSpeed *= driftTurnMultiplier;
            }

            float turnAmount = turnInput * currentTurnSpeed * directionFactor * Time.fixedDeltaTime;

            Quaternion rot = rb.rotation * Quaternion.Euler(0f, turnAmount, 0f);
            rb.MoveRotation(rot);
        }
    }
}