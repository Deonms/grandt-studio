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
            if (point != null && point.transform.childCount == 0)
            {
                emptySpots.Add(point);
            }
        }

        if (emptySpots.Count == 0)
            return;

        int randomIndex = Random.Range(0, emptySpots.Count);
        _activeSpot = emptySpots[randomIndex];
        _activeSpot.SetActive(true);
    }

    private void Start()
    {
        if (_shelfSpots == null)
            return;

        foreach (GameObject point in _shelfSpots)
        {
            if (point != null)
            {
                point.SetActive(false);
            }
        }

        ActivateNextSpot();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject box = collision.gameObject;

        if (box.CompareTag("Fragile") || box.CompareTag("heavy") || box.CompareTag("light") || box.CompareTag("liquid") || box.CompareTag("normal"))
        {
            if (_activeSpot != null && _activeSpot.transform.childCount == 0)
            {
                // Save the box size before moving it
                Vector3 boxSize = box.transform.lossyScale;

                // Move the box to the spot
                box.transform.SetParent(_activeSpot.transform);
                box.transform.position = _activeSpot.transform.position;
                box.transform.rotation = _activeSpot.transform.rotation;

                // Keep the same size after parenting
                Vector3 parentScale = _activeSpot.transform.lossyScale;
                box.transform.localScale = new Vector3(
                    boxSize.x / parentScale.x,
                    boxSize.y / parentScale.y,
                    boxSize.z / parentScale.z
                );

                _activeSpot = null;

                if (!_isWaitingForNextSpot)
                {
                    StartCoroutine(ActivateNextPointAfterDelay());
                }
            }
        }
    }

    public void RemoveBoxFromSpot(GameObject spot)
    {
        if (spot != null)
        {
            if (spot.transform.childCount > 0)
            {
                spot.transform.GetChild(0).SetParent(null);
            }

            spot.SetActive(false);

            if (!_isWaitingForNextSpot)
            {
                StartCoroutine(ActivateNextPointAfterDelay());
            }
        }
    }
}