// Package.cs  (put on each package, needs a Rigidbody + Collider)
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Package : MonoBehaviour, IInteractable
{
    public string packageId = "Package";
    public float weight = 5f;          // slows the player
    public string destinationId;       // matches a DeliveryZone id

    public string Prompt => $"Pick up {packageId} [E]";
    public void Interact(PlayerInteractor player) => player.PickUp(this);
}