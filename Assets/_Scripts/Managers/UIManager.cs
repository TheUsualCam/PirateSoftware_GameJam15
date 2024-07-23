using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateTimerUI(int mins, int secs)
    {
        string formattedMins;
        string formattedSecs;

        if (mins < 10)
        {
            formattedMins = "0" + mins.ToString();
        }
        else
        {
            formattedMins = mins.ToString();
        }

        if (secs < 10)
        {
            formattedSecs = "0" + secs.ToString();
        }
        else
        {
            formattedSecs = secs.ToString();
        }

        timerText.text = formattedMins + " : " + formattedSecs;
    }
}
