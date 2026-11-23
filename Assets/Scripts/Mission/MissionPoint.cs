using UnityEngine;

public class MissionPoint : MonoBehaviour
{
    [Header("References")]
    public MissionManager manager;
    [Header("Interact Settings")]
    public KeyCode interactKey = KeyCode.E;
    private bool _playerInRange = false;

    void OnEnable()
    {
        _playerInRange = false;
    }
    void OnDisable()
    {
        _playerInRange = false;
    }
    void Update()
    {
        if (!_playerInRange || manager == null) return;

        if (Input.GetKeyDown(interactKey))
        {
            _playerInRange = false;
            manager.CompleteMissionPoint();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = false;
    }
}