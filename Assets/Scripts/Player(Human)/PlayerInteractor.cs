// PlayerInteractor.cs  (put on Player)
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform holdPoint;       // empty child of camera at ~(0.3, -0.3, 0.8)
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private float throwForce = 6f;
    [SerializeField] private FirstPersonController controller;

    public Package Held { get; private set; }
    public string CurrentPrompt { get; private set; } // read this from your UI

    private void Update()
    {
        CurrentPrompt = "";
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward,
            out RaycastHit hit, interactRange, ~0, QueryTriggerInteraction.Collide)
            && hit.collider.GetComponentInParent<IInteractable>() is IInteractable target
            && Held == null)
        {
            CurrentPrompt = target.Prompt;
            if (Input.GetKeyDown(KeyCode.E)) target.Interact(this);
        }

        if (Held != null)
        {
            if (Input.GetKeyDown(KeyCode.G)) Drop(false);
            if (Input.GetMouseButtonDown(0)) Drop(true);
        }
    }

    public void PickUp(Package p)
    {
        Held = p;
        var rb = p.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        foreach (var c in p.GetComponentsInChildren<Collider>()) c.enabled = false;
        p.transform.SetParent(holdPoint);
        p.transform.localPosition = Vector3.zero;
        p.transform.localRotation = Quaternion.identity;
        controller.SpeedMultiplier = Mathf.Clamp(1f - p.weight * 0.03f, 0.4f, 1f);
    }

    public void Drop(bool throwIt)
    {
        if (Held == null) return;
        var p = Held;
        Held = null;
        p.transform.SetParent(null);
        foreach (var c in p.GetComponentsInChildren<Collider>()) c.enabled = true;
        var rb = p.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        if (throwIt) rb.AddForce(cameraTransform.forward * throwForce, ForceMode.VelocityChange);
        controller.SpeedMultiplier = 1f;
    }
}