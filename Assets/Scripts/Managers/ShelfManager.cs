using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    [Header("Shelf Positions")]
    private List<GameObject> _shelfSpots;
    private float _delayBeforeNextSpot = 3f;
    private GameObject _activeSpot;
    private bool _isWaitingForNextSpot = false;
    private List<GameObject> _typeOfBoxes;

    private IEnumerator ActivateNextPointAfterDelay()
    {
        _isWaitingForNextSpot = true;
        yield return new WaitForSeconds(_delayBeforeNextSpot);
        _isWaitingForNextSpot = false;
        ActivateNextSpot();
    }

    private void ActivateNextSpot()
    {
        if (_shelfSpots == null || _shelfSpots.Count == 0) return;

        GameObject nextPoint = _activeSpot;

        if (_shelfSpots.Count > 1)
        {
            int safetyCounter = 0;
            while (nextPoint == _activeSpot && safetyCounter < 50)
            {
                int randomIndex = Random.Range(0, _shelfSpots.Count);
                nextPoint = _shelfSpots[randomIndex];
                safetyCounter++;
            }
        }
        else
        {
            nextPoint = _shelfSpots[0];
        }
        _activeSpot = nextPoint;
        if (_activeSpot != null)
        {
            _activeSpot.SetActive(true);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject point in _shelfSpots)
        {
            if (point != null)
            {
                point.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
            
    }
}
