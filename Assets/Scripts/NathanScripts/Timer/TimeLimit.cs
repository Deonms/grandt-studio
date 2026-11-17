using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class TimeLimit : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _timeRemaining;
    [SerializeField] private int _decimals;


    private void Update()
    {
        if (_timeRemaining < 0) return;
        if(_timeRemaining < 0)
        {
            _timeRemaining = 0;
        }
        _timeRemaining -= Time.deltaTime;
        UpdateTimer();
    }


    private void UpdateTimer()
    {
        _text.text = _timeRemaining.ToString($"F{_decimals}");
    }
}
