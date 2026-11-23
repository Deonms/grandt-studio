using UnityEngine;

public class CanvasTimer : MonoBehaviour
{
    public PauzeManager screenManager;
    public float duration = 20f;

    private float timer;
    private bool finished = false;

    void Start()
    {
        timer = duration;
    }

    void Update()
    {
        if (finished) return;

        timer -= Time.deltaTime;

        if (timer <= 0f || Input.GetKeyDown(KeyCode.Q))
        {
            FinishIntro();
        }
    }

    public void FinishIntro()
    {
        if (finished) return;
        finished = true;

        if (screenManager != null)
        {
            screenManager.OnIntroFinished();
        }

        gameObject.SetActive(false);
    }
}