using UnityEngine;


/// <summary>
/// GlobalTimer
/// 
/// Keeps track of how long the player takes to finish the whole game, from the start of scanning to the end of delivery.
///
/// This is a static class because when the game switches scenes (sorting scene -> DeliveryScene) a standard timer would reset. 
/// 
/// When Unity loads a new scene,it destroys every GameObject in the old one, including any scripts on them.
/// A static class is always active, as a global timer should.
///
/// Why doesn't it count up every frame?
/// Counting with Time.deltaTime needs Update(), and Update() only runs on a MonoBehaviour.
/// So instead, it remembers when the timer started (Time.time) and subtracts that from the current Time.time whenever something asks for the elapsed time.
/// 
/// 
/// Time.time is "scaled" game time: it stands still while Time.timeScale is 0, so pausing the game (PauzeManager) pauses the global timer as well.
/// </summary>
public static class GlobalTimer
{
    // The value of Time.time at the moment the timer started
    private static float _startTime;

    // The total time, saved when the timer stops, so it doesn't keep growing afterwards
    private static float _finalTime;

    private static bool _isRunning;
    private static bool _hasFinished;

    public static bool IsRunning => _isRunning;

    // True once the game has been completed and the final time is known
    public static bool HasFinished => _hasFinished;

    // Total seconds so far.
    public static float ElapsedTime
    {
        get
        {
            if (_isRunning)
            {
                return Time.time - _startTime;
            }
            return _finalTime;
        }
    }

    // Call this when the game starts.
    // Calling it again restarts from 0, which is what you want when a new player starts.
    public static void StartTimer()
    {
        _startTime = Time.time;
        _finalTime = 0f;
        _isRunning = true;
        _hasFinished = false;
    }

    // Call this when the game ends (delivery section is over).
    public static void StopTimer()
    {
        // Stopping twice would overwrite the real final time, so ignore it if it's already stopped
        if (!_isRunning) return;

        _finalTime = Time.time - _startTime;
        _isRunning = false;
        _hasFinished = true;
    }

    // Back to 0 and not running, like before the game started.
    public static void ResetTimer()
    {
        _startTime = 0f;
        _finalTime = 0f;
        _isRunning = false;
        _hasFinished = false;
    }

    // Safety net for the editor.
    // Right now the project reloads everything when you press Play (Project Settings > Editor > Enter Play Mode Settings).
    // If someone changes that to skip the "Domain" reload to make. Play start faster, static variables keep their values from the previous time you pressed Play.
    // RuntimeInitializeOnLoadMethod makes Unity call this method automatically every time the game starts, so the timer always starts clean either way.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnGameStart()
    {
        ResetTimer();
    }
}
