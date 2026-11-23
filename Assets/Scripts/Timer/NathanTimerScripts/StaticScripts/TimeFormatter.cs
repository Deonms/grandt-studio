using UnityEngine;

/// <summary>
/// TimeFormatter
/// 
/// Turns a number of seconds into text like "1:05".
///
/// This is a static class. 
/// So, you never have to put it on a GameObject and never create one with "new".
/// Any script can call TimeFormatter.ToMinutesAndSeconds(...) directly.
/// The countdown timer and the global timer both use it, so they always look the same.
/// </summary>
/// 
public static class TimeFormatter
{
    // A countdown should keep showing "0:01" until the last second is completely over,
    // and only show "0:00" when the time is really up. So it rounds up (0.4 seconds left = "0:01").
    // A stopwatch, like the global timer, rounds down like a normal clock (59.9 seconds = "0:59").
    public static string ToMinutesAndSeconds(float seconds, bool roundUp = false)
    {
        if (seconds < 0f) seconds = 0f;

        int totalSeconds = roundUp ? Mathf.CeilToInt(seconds) : Mathf.FloorToInt(seconds);

        // Whole-number division: 125 / 60 = 2 (the remainder is dropped)
        int minutes = totalSeconds / 60;

        // % gives the remainder: 125 % 60 = 5
        int remainingSeconds = totalSeconds % 60;

        // D2 means "always at least 2 digits", so 5 seconds shows as "05" and you get "2:05"
        return $"{minutes}:{remainingSeconds:D2}";
    }
}
