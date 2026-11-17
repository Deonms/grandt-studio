
using UnityEngine;

public class CanvasTimer : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(DisableCanvas), 20f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DisableCanvas();
        }
    }
    void DisableCanvas()
    {
        gameObject.SetActive(false);
    }
}
