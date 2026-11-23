using UnityEngine;

/// <summary>
/// GlobalTimerControls
/// 
/// 
/// A bridge between the Inspector and the static GlobalTimer.
///
/// UnityEvents (like On Time Up on a CountdownTimer, or a button's On Click) can only call methods on components that exist in the scene. 
/// GlobalTimer is static, so it isn't in the scene and an event can't select it. 
/// 
/// This component sits on a GameObject and passes the call on to GlobalTimer.
/// Other scripts can skip this and call GlobalTimer.StartTimer() / GlobalTimer.StopTimer() directly.
/// </summary>
public class GlobalTimerControls : MonoBehaviour
{
    // Only turn this on in the scene where the game starts (or in a test scene).
    // If it's on in a later scene, the total time would restart there.
    [SerializeField] private bool _startOnSceneStart = false;

    private void Start()
    {
        if (_startOnSceneStart)
        {
            StartGlobalTimer();
        }
    }

    public void StartGlobalTimer()
    {
        GlobalTimer.StartTimer();
        Debug.Log("[GlobalTimer] Started");
    }

    public void StopGlobalTimer()
    {
        GlobalTimer.StopTimer();

        // F2 = 2 decimals, for checking the exact time in the Console
        Debug.Log($"[GlobalTimer] Stopped at {TimeFormatter.ToMinutesAndSeconds(GlobalTimer.ElapsedTime)} ({GlobalTimer.ElapsedTime:F2} seconds)");
    }
}
