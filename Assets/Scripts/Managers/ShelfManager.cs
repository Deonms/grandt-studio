using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    [Header("Shelf Positions")]
    public List<GameObject> shelfSpots;
    public float delayBeforeNextSpot = 3f;
    private GameObject _activeSpot;
    private bool _isWaitingForNextSpot = false;

    private IEnumerator ActivateNextPointAfterDelay()
    {
        _isWaitingForNextSpot = true;
        yield return new WaitForSeconds(delayBeforeNextSpot);
        _isWaitingForNextSpot = false;
        ActivateNextPoint();
    }

    private void ActivateNextPoint()
    {
        if (shelfSpots == null || shelfSpots.Count == 0) return;

        GameObject nextPoint = _activeSpot;

        if (shelfSpots.Count > 1)
        {
            int safetyCounter = 0;
            while (nextPoint == _activeSpot && safetyCounter < 50)
            {
                int randomIndex = Random.Range(0, shelfSpots.Count);
                nextPoint = shelfSpots[randomIndex];
                safetyCounter++;
            }
        }
        else
        {
            nextPoint = shelfSpots[0];
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
