using UnityEngine;

public class CarLogic : MonoBehaviour
{
    [Header("Movement")]
    public Transform[] waypoints;
    public float speed = 10f;
    public float turnSpeed = 6f;
    public float reachDistance = 0.5f;
    public bool loop = true;

    [Header("Grandma & Stoplight")]
    public bool isGrandma = false;
    public float grandmaSpeed = 18f;
    public Stoplight stoplight;

    private int currentIndex = 0;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0 || currentIndex >= waypoints.Length)
            return;

        Transform target = waypoints[currentIndex];
        if (target == null) return;

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;

        if (toTarget.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(toTarget.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);
        }

        float currentSpeed = GetCurrentSpeed();

        Vector3 nextPos = Vector3.MoveTowards(
            transform.position,
            new Vector3(target.position.x, transform.position.y, target.position.z),
            currentSpeed * Time.deltaTime
        );
        transform.position = nextPos;

        float flatDistance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(target.position.x, target.position.z)
        );

        if (flatDistance <= reachDistance)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                currentIndex = loop ? 0 : waypoints.Length;
            }
        }
    }

    private float GetCurrentSpeed()
    {
        if (isGrandma && stoplight != null && stoplight.IsThirdLightActive)
        {
            return grandmaSpeed;
        }

        return speed;
    }
}