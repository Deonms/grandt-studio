using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TimeLimit : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _timeRemaining;



    private void Update()
    {
        if (_timeRemaining < 0) return;

        _timeRemaining -= Time.deltaTime;
        UpdateTimer();
    }


    private void UpdateTimer()
    {
        _text.text = _timeRemaining.ToString();
    }
}
