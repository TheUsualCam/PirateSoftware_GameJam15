using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private TextMeshProUGUI recipesCompletedText;
    [SerializeField] private float textDisplayDuration = 2.0f;
    [SerializeField] private GameObject recipeWindow;
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private List<TextMeshProUGUI> requiredIngredientsText = new List<TextMeshProUGUI>();
    [SerializeField] private Slider cauldronCorruptionSlider;

    private void Start()
    {
        notificationText.enabled = false;
        gameOverWindow.SetActive(false);
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
            if (i < recipe.requiredIngredients.Count && !recipe.requiredIngredients[i].isInCauldron)
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

    public void UpdateCauldronCorruptionUI(float cauldronCorruption)
    {
        cauldronCorruptionSlider.value = cauldronCorruption;
    }

    public void DisplayNotificationText(bool recipeCompleted)
    {
        if(recipeCompleted)
        {
            notificationText.text = "Recipe Completed!";
        }
        else
        {
            notificationText.text = "Cauldron Corrupted!";
        }

        notificationText.enabled = true;
        StartCoroutine(DeactivateTextAfterTime());
    }

    IEnumerator DeactivateTextAfterTime()
    {
        yield return new WaitForSeconds(textDisplayDuration);
        notificationText.enabled = false;
    }

    public void GameOverUI(int recipesCompleted)
    {
        timerText.enabled = false;
        recipeWindow.SetActive(false);
        notificationText.enabled = false;
        gameOverWindow.SetActive(true);
        recipesCompletedText.text = "Recipes Completed: " + recipesCompleted;
    }

    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
