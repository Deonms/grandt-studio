using TMPro;
using UnityEngine;

/// <summary>
/// GlobalTimerDisplay
/// 
/// Shows the global timer on a text. 
/// It only reads GlobalTimer and never changes it, so you can put one in every scene that should show the total time, including the end screen.
/// While the timer runs, the text goes up live. After StopTimer() it shows the final time.
/// </summary>
public class GlobalTimerDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    // Text in front of the time. For example "Total time: " during the game, and "Your time: " on the end screen.
    [SerializeField] private string _prefix = "Total time: ";

    private void Update()
    {
        if (_text == null) return;

        // No roundUp here: the global timer counts up like a stopwatch, so it rounds down
        _text.text = _prefix + TimeFormatter.ToMinutesAndSeconds(GlobalTimer.ElapsedTime);
    }
}
