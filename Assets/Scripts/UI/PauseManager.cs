using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScreenManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject menuUI;

    private bool isGameStopped = false;

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
    public void PauseGame()
    {
        isGameStopped = true;
        Time.timeScale = 0f;

        if (menuUI != null)
        {
            menuUI.SetActive(true);
        }
    }
    public void ResumeGame()
    {
        isGameStopped = false;
        Time.timeScale = 1f;

        if (menuUI != null)
        {
            menuUI.SetActive(false);
        }
    }
}