using UnityEngine;

/// <summary>
/// SceneFlowControls
/// 
/// The same idea as GlobalTimerControls: SceneLoader is static, so buttons and other UnityEvents (like a CountdownTimer's On Time Up) can't select it in the Inspector.
/// This component sits in the scene and passes the call on.
/// The methods have no parameters on purpose: that way they show up in the event dropdown without having to type a scene name into the Inspector.
/// 
/// </summary>
//

public class SceneFlowControls : MonoBehaviour
{
    // Tick this in the start menu and end screen.
    // The first-person controller locks and hides the mouse (in its Awake), and the lock carries over to the next scene.
    // Without this, you couldn't click any buttons.
    [SerializeField] private bool _showCursorOnStart = false;

    private void Start()
    {
        if (_showCursorOnStart)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LoadStartMenu() => SceneLoader.LoadScene(SceneNames.StartMenu);
    public void LoadWarehouse() => SceneLoader.LoadScene(SceneNames.Warehouse);
    public void LoadDelivery() => SceneLoader.LoadScene(SceneNames.Delivery);
    public void LoadEndScreen() => SceneLoader.LoadScene(SceneNames.EndScreen);
    public void ReloadCurrentScene() => SceneLoader.ReloadCurrentScene();

    public void QuitGame()
    {
        // Application.Quit does nothing inside the editor, so the log shows that the button works
        Debug.Log("[SceneFlowControls] Quit");
        Application.Quit();
    }
}
