using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Manager References")]
    [SerializeField] private UIManager uiManager;

    [Space, Header("Game State Parameters")]
    [SerializeField] private float gameTimerInMinutes = 3.0f;

    private float timerMins = 0;
    private float timerSecs = 0;

    // Start is called before the first frame update
    void Start()
    {
        timerMins = gameTimerInMinutes;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameTimer();
    }

    void UpdateGameTimer()
    {
        if (timerSecs < 0)
        {
            if (timerMins > 0)
            {
                timerMins--;
                timerSecs = 59;
            }
            else
            {
                timerMins = 0;
                timerSecs = 0;
                Debug.Log("Game Over!");
            }
        }
        else
        {
            timerSecs -= Time.deltaTime;
        }

        uiManager.UpdateTimerUI((int)timerMins, (int)timerSecs);
    }
}
