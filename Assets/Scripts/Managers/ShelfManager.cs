using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    [Header("Shelf Positions")]
    public List<GameObject> shelfSpots;
    public float delayBeforeNextPoint = 3f;
    private bool _isWaitingForNextSpot = false;

    private IEnumerator ActivateNextPointAfterDelay()
    {
        _isWaitingForNextSpot = true;
        yield return new WaitForSeconds(delayBeforeNextPoint);
        _isWaitingForNextSpot = false;
        ActivateNextPoint();
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
