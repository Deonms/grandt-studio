using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneLoader
/// 
/// Every scene change in the game goes through here, so the same safety checks
/// happen every time, no matter who loads a scene.
///
/// Static class, like GlobalTimer and TimeFormatter: call SceneLoader.LoadScene(...) from any script.
/// </summary>

public static class SceneLoader
{
    public static void LoadScene(string sceneName)
    {
        // Unity can only load scenes that are in the build's scene list.
        // Without this check you'd get a vague error. With it, the Console says exactly what's wrong.
        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"[SceneLoader] Can't load \"{sceneName}\". Check that the name in SceneNames matches the scene file exactly, " +
                           "and that the scene is in File > Build Profiles > Scene List.");
            return;
        }

        // If the game was paused (PauzeManager sets Time.timeScale to 0) and a scene gets loaded, timeScale stays 0 in the new scene and everything is frozen.
        // Setting it back to 1 here prevents that, whatever loaded the scene.
        Time.timeScale = 1f;

        Debug.Log($"[SceneLoader] Loading {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // Loads the scene you're in again from the start.
    public static void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
}
