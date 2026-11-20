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
    private GameObject activePoint;
    private bool isWaitingForNextPoint = false;

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
        if (isWaitingForNextPoint) return;
        currentScore++;
        UpdateScoreUI();

        if (activePoint != null)
        {
            activePoint.SetActive(false);
        }
        StartCoroutine(ActivateNextPointAfterDelay());
    }
    private IEnumerator ActivateNextPointAfterDelay()
    {
        isWaitingForNextPoint = true;
        yield return new WaitForSeconds(delayBeforeNextPoint);
        isWaitingForNextPoint = false;
        ActivateNextPoint();
    }
    private void ActivateNextPoint()
    {
        if (missionPoints == null || missionPoints.Count == 0) return;

        GameObject nextPoint = activePoint;

        if (missionPoints.Count > 1)
        {
            int safetyCounter = 0;
            while (nextPoint == activePoint && safetyCounter < 50)
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
        activePoint = nextPoint;
        if (activePoint != null)
        {
            activePoint.SetActive(true);
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