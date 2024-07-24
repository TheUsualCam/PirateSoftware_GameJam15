using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private List<TextMeshProUGUI> requiredIngredientsText = new List<TextMeshProUGUI>();

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

    public void UpdateRecipeUI(Recipe recipe)
    {
        for (int i = 0; i < requiredIngredientsText.Count; i++)
        {
            if (i < recipe.requiredIngredients.Count)
            {
                requiredIngredientsText[i].enabled = true;
                requiredIngredientsText[i].text = recipe.requiredIngredients[i].ingredient.ingredientName + ", " + recipe.requiredIngredients[i].method;
            }
            else
            {
                requiredIngredientsText[i].enabled = false;
            }
        }
    }
}
