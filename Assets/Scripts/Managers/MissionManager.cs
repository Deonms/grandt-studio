using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [Header("Mission Points")]
    public List<GameObject> missionPoints;
    public float delayBeforeNextPoint = 3f;

    [Header("UI")]
    public TMP_Text scoreText;
    public string scorePrefix = "Score: ";

    public int currentScore = 0;
    private GameObject _activePoint;
    private bool _isWaitingForNextPoint = false;

    public Transform ActiveTarget => (!_isWaitingForNextPoint && _activePoint != null) ? _activePoint.transform : null;

    void Start()
    {
        foreach (GameObject point in missionPoints)
        {
            if (point != null)
            {
                point.SetActive(false);
            }
        }
        UpdateScoreUI();
        ActivateNextPoint();
    }
    public void CompleteMissionPoint()
    {
        if (_isWaitingForNextPoint) return;
        currentScore++;
        UpdateScoreUI();

        if (_activePoint != null)
        {
            _activePoint.SetActive(false);
        }
        StartCoroutine(ActivateNextPointAfterDelay());
    }
    private IEnumerator ActivateNextPointAfterDelay()
    {
        _isWaitingForNextPoint = true;
        yield return new WaitForSeconds(delayBeforeNextPoint);
        _isWaitingForNextPoint = false;
        ActivateNextPoint();
    }
    private void ActivateNextPoint()
    {
        if (missionPoints == null || missionPoints.Count == 0) return;

        GameObject nextPoint = _activePoint;

        if (missionPoints.Count > 1)
        {
            int safetyCounter = 0;
            while (nextPoint == _activePoint && safetyCounter < 50)
            {
                int randomIndex = Random.Range(0, missionPoints.Count);
                nextPoint = missionPoints[randomIndex];
                safetyCounter++;
            }
        }
        else
        {
            nextPoint = missionPoints[0];
        }
        _activePoint = nextPoint;
        if (_activePoint != null)
        {
            _activePoint.SetActive(true);
        }
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = scorePrefix + currentScore;
        }
    }
}