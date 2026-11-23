using UnityEngine;

public class CompassBarUI : MonoBehaviour
{
    [Header("References")]
    public MissionManager missionManager;
    public Transform playerReference; //Player is needed for location
    public RectTransform compassBarRect;
    public RectTransform targetMarker;

    [Header("Compass Settings")]
    [Tooltip("How many degrees to the left and right of center are shown on the bar (e.g., 90 = 180 degrees total).")]
    public float halfViewAngle = 90f;
    [Tooltip("If true, the marker stays clamped to the left/right edge when outside range. If false, it hides outside range.")]
    public bool clampToEdges = true;
    [Tooltip("Optional padding in pixels from the left and right edges of the compass bar.")]
    public float edgePadding = 12f;
    [Tooltip("Optional: slightly scale the marker as you get closer to the objective.")]
    public bool scaleByDistance = false;
    public float maxScaleDistance = 50f;
    public float minScale = 0.75f;
    public float maxScale = 1.15f;

    void Awake()
    {
        if (playerReference == null && Camera.main != null)
        {
            playerReference = Camera.main.transform;
        }

        if (targetMarker != null)
        {
            targetMarker.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (missionManager == null || playerReference == null || compassBarRect == null || targetMarker == null)
            return;

        Transform target = missionManager.ActiveTarget;
        if (target == null)
        {
            if (targetMarker.gameObject.activeSelf)
                targetMarker.gameObject.SetActive(false);
            return;
        }

        // Flatten to the horizontal XZ plane so elevation differences don't skew horizontal bearing
        Vector3 toTarget = target.position - playerReference.position;
        toTarget.y = 0f;

        Vector3 forward = playerReference.forward;
        forward.y = 0f;

        if (toTarget.sqrMagnitude < 0.0001f || forward.sqrMagnitude < 0.0001f)
            return;

        toTarget.Normalize();
        forward.Normalize();

        // Signed angle in degrees (-180 = far left, +180 = far right)
        float signedAngle = Vector3.SignedAngle(forward, toTarget, Vector3.up);

        // Normalized X position: -1 (left edge) to +1 (right edge)
        float normalizedX = signedAngle / halfViewAngle;

        bool isWithinRange = Mathf.Abs(normalizedX) <= 1f;

        if (!isWithinRange && !clampToEdges)
        {
            if (targetMarker.gameObject.activeSelf)
                targetMarker.gameObject.SetActive(false);
            return;
        }
        if (!targetMarker.gameObject.activeSelf)
            targetMarker.gameObject.SetActive(true);
        float clampedNormalizedX = Mathf.Clamp(normalizedX, -1f, 1f);
        float halfWidth = (compassBarRect.rect.width * 0.5f) - edgePadding;
        float posX = clampedNormalizedX * halfWidth;
        Vector2 anchoredPos = targetMarker.anchoredPosition;
        anchoredPos.x = posX;
        targetMarker.anchoredPosition = anchoredPos;

        if (scaleByDistance)
        {
            float distance = Vector3.Distance(playerReference.position, target.position);
            float t = 1f - Mathf.Clamp01(distance / maxScaleDistance);
            float scale = Mathf.Lerp(minScale, maxScale, t);
            targetMarker.localScale = Vector3.one * scale;
        }
    }
}