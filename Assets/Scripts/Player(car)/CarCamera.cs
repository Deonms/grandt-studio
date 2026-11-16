using UnityEngine;

public class CarCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 3.5f, -7f);
    public float positionSmoothSpeed = 10f;
    public float rotationSmoothSpeed = 10f;
    void LateUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.TransformPoint(offset);
        transform.position = Vector3.Lerp(transform.position, targetPosition, positionSmoothSpeed * Time.deltaTime);
        Vector3 lookAtTarget = target.position + target.forward * 2f + Vector3.up * 1.5f;
        Quaternion targetRotation = Quaternion.LookRotation(lookAtTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}