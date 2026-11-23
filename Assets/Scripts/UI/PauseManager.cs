using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject menuUI;
    public GameObject missionUI;
    public GameObject introCanvas;

    private bool isGameStopped = false;
    private bool introFinished = false;

    void Start()
    {
        // Zorg dat de missie-UI uit staat zolang de intro loopt
        if (introCanvas != null && introCanvas.activeInHierarchy)
        {
            introFinished = false;
            SetMissionUIVisible(false);
        }
        else
        {
            introFinished = true;
            RefreshMissionUI();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameStopped)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void OnIntroFinished()
    {
        introFinished = true;
        RefreshMissionUI();
    }

    public void PauseGame()
    {
        isGameStopped = true;
        Time.timeScale = 0f;

        if (menuUI != null)
        {
            menuUI.SetActive(true);
        }

        RefreshMissionUI();
    }

    public void ResumeGame()
    {
        isGameStopped = false;
        Time.timeScale = 1f;

        if (menuUI != null)
        {
            menuUI.SetActive(false);
        }

        RefreshMissionUI();
    }

    private void RefreshMissionUI()
    {
        bool shouldShow = introFinished && !isGameStopped;
        SetMissionUIVisible(shouldShow);
    }

    private void SetMissionUIVisible(bool visible)
    {
        if (missionUI != null)
        {
            missionUI.SetActive(visible);
        }
    }
}