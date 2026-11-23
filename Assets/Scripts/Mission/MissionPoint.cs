using UnityEngine;

public class MissionPoint : MonoBehaviour
{
    public MissionManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && manager != null)
        {
            manager.CompleteMissionPoint();
        }
    }
}