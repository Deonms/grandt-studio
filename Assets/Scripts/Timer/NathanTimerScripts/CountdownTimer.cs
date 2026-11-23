using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


/// <summary>
/// CountdownTimer
/// Counts down from a set number of seconds and shows it on screen.
/// When it reaches 0 it fires the "On Time Up" event, once.
/// You use one CountdownTimer per section: scanning, sorting and delivery.
/// 
/// The timer itself doesn't know what a "section" is. 
/// Whatever should happen when time runs out gets hooked up in the Inspector through On Time Up.
/// That way everyone can connect their own code to it without anyone having to change this script.
/// </summary> 
public class CountdownTimer : MonoBehaviour
{
    [Header("Settings")]
    // FormerlySerializedAs
    // This attribute tells Unity to copy the old saved value (60 in the TimerCanvas prefab) into this renamed field.
    [FormerlySerializedAs("_timeRemaining")]
    [SerializeField] private float _duration = 60f;

    // True: the countdown starts by itself when the scene starts.
    // False: something else has to call StartCountdown() first.
    // Turn it off when the countdown should wait for something, like an intro screen.
    [SerializeField] private bool _startOnSceneStart = true;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _text;

    [Header("Events")]
    // Fires once when the countdown reaches 0.
    // A UnityEvent shows up in the Inspector like a button's On Click list.
    [SerializeField] private UnityEvent _onTimeUp;

    // The time left is no longer filled in by hand in the Inspector.
    // It's set from _duration when the countdown starts.
    private float _timeRemaining;
    private bool _isRunning;

    // Read-only properties: other scripts can read these, but can't change them.
    public float TimeRemaining => _timeRemaining;
    public bool IsRunning => _isRunning;

    private void Start()
    {
        _timeRemaining = _duration;
        UpdateText();

        if (_startOnSceneStart)
        {
            StartCountdown();
        }
    }

    private void Update()
    {
        if (!_isRunning) return;

        // Time.deltaTime is 0 while the game is paused (PauseManager sets Time.timeScale to 0), so the countdown pauses automatically.
        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0f)
        {
            // Set to 0 so the screen never shows a negative time.
            _timeRemaining = 0f;

            // Stop before invoking the event, so the time-up code can only ever run once.
            _isRunning = false;
            UpdateText();
            _onTimeUp.Invoke();
            return;
        }

        UpdateText();
    }

    // Starts (or restarts) the countdown from the full duration.
    // I made this public so that it can be called from another script or picked in a UnityEvent.
    public void StartCountdown()
    {
        _timeRemaining = _duration;
        _isRunning = true;
        UpdateText();
    }

    // Stops the countdown without activating On Time Up. For when the player finishes a section before the time runs out.
    public void StopCountdown()
    {
        _isRunning = false;
    }

    private void UpdateText()
    {
        if (_text == null) return;

        // roundUp: true, so the last second shows "0:01" and "0:00" only appears.
        // When time is actually up. See the TimeFormatter script for the details.
        _text.text = TimeFormatter.ToMinutesAndSeconds(_timeRemaining, roundUp: true);
    }
}
