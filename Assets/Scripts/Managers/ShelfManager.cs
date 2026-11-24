using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    [Header("Shelf Positions")]
    [SerializeField] private List<GameObject> _shelfSpots;
    [SerializeField] private List<GameObject> _typeOfBoxes;

    private float _delayBeforeNextSpot = 3f;
    private GameObject _activeSpot;
    private bool _isWaitingForNextSpot = false;

    //spots to store the boxes
    private Dictionary<GameObject, GameObject> _boxSpots =
        new Dictionary<GameObject, GameObject>();

    private IEnumerator ActivateNextPointAfterDelay()
    {
        _isWaitingForNextSpot = true;

        yield return new WaitForSeconds(_delayBeforeNextSpot);

        _isWaitingForNextSpot = false;
        ActivateNextSpot();
    }

    private void ActivateNextSpot()
    {
        if (_shelfSpots == null || _shelfSpots.Count == 0)
            return;

        List<GameObject> emptySpots = new List<GameObject>();

        foreach (GameObject point in _shelfSpots)
        {
            if (point != null && !IsSpotOccupied(point))
            {
                emptySpots.Add(point);
            }
        }

        if (emptySpots.Count == 0)
        {
            _activeSpot = null;
            return;
        }

        int randomIndex = Random.Range(0, emptySpots.Count);
        _activeSpot = emptySpots[randomIndex];

        
        _activeSpot.SetActive(false);
    }

    private bool IsSpotOccupied(GameObject spot)
    {
        foreach (GameObject box in _boxSpots.Keys)
        {
            if (box != null && _boxSpots[box] == spot)
                return true;
        }

        return false;
    }

    private void Start()
    {
        if (_shelfSpots == null)
            return;

        foreach (GameObject point in _shelfSpots)
        {
            if (point != null)
                point.SetActive(false);
        }

        ActivateNextSpot();
    }

    private void OnCollisionEnter(Collision collision) //this makes sure the child of the manager also works on the collission trigger
    {
        TryMoveBoxToSpot(collision.gameObject);
    }

    private void OnTransformChildrenChanged()
    {
        //checks for children
    }

    private void TryMoveBoxToSpot(GameObject box)
    {
        if (box == null || _activeSpot == null)
            return;

        if (_activeSpot == box || IsSpotOccupied(_activeSpot)) //makes sure the box doesnt change size
            return;

        if (!IsBox(box))
            return;


        Vector3 boxSize = box.transform.lossyScale;

        //to moce the boxes to the spots set
        box.transform.SetParent(null);
        box.transform.position = _activeSpot.transform.position;
        box.transform.rotation = _activeSpot.transform.rotation;
        box.transform.localScale = boxSize;
    
       
        _boxSpots[box] = _activeSpot; //checks for empty spots

        _activeSpot = null;

        if (!_isWaitingForNextSpot)
            StartCoroutine(ActivateNextPointAfterDelay());
    }

    private bool IsBox(GameObject box)
    {
        if (box.CompareTag("Fragile") ||
            box.CompareTag("heavy") ||
            box.CompareTag("light") ||
            box.CompareTag("liquid") ||
            box.CompareTag("normal"))
        {
            return true;
        }

        return false;
    }

    public void RemoveBoxFromSpot(GameObject box)
    {
        if (box == null)
            return;

        if (_boxSpots.ContainsKey(box))
            _boxSpots.Remove(box);

        if (!_isWaitingForNextSpot)
            StartCoroutine(ActivateNextPointAfterDelay());
    }
}