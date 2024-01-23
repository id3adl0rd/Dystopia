using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Timer : MonoBehaviour
{
    private float _timeRemaining = 10;
    private bool _timerIsRunning = false;
    private void Start()
    {
        _timerIsRunning = true;
    }

    public void SetTimer(float timeRemaining)
    {
        _timeRemaining = timeRemaining;
    }
    void Update()
    {
        if (_timerIsRunning)
        {
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                _timeRemaining = 0;
                _timerIsRunning = false;
            }
        }
    }
}