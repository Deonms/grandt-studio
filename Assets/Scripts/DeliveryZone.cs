// DeliveryZone.cs  (empty GameObject with a trigger BoxCollider at each door)
using UnityEngine;

public class DeliveryZone : MonoBehaviour
{
    public string zoneId;

    private void OnTriggerEnter(Collider other)
    {
        var pkg = other.GetComponentInParent<Package>();
        if (pkg != null && pkg.destinationId == zoneId)
        {
            Debug.Log($"Delivered {pkg.packageId}!"); // hook up score/money/timer here
            Destroy(pkg.gameObject);
        }
    }
}