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
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float textDisplayDuration = 2.0f;
    [SerializeField] private GameObject recipeWindow;
    [SerializeField] private GameObject gameOverWindow;
    [SerializeField] private Slider cauldronCorruptionSlider;
    
    [Header("Recipe Cards")]
    [SerializeField] private List<RecipeCard> recipeCards = new List<RecipeCard>();
    [SerializeField] private Transform recipeCardContainer;
    [SerializeField] private GameObject[] recipeCardPrefabs;

    [Space, Header("Audio")]
    [SerializeField] private AudioClip uiClip;

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

    public void CreateNewRecipeCardUI(Recipe recipe)
    {
        RecipeCard newRecipeCard = Instantiate(recipeCardPrefabs[recipe.requiredIngredients.Count-1], recipeCardContainer).GetComponent<RecipeCard>();
        recipeCards.Add(newRecipeCard);
        newRecipeCard.Initialize(recipe);
    }

    public void UpdateCard(Recipe updatedRecipe)
    {
        foreach (RecipeCard card in recipeCards)
        {
            if (card.recipe == updatedRecipe)
            {
                card.UpdateCard(updatedRecipe);
                break;
            }
        }
    }

    public void CloseRecipeCard(Recipe recipeToClose)
    {
        RecipeCard cardToClose = null;
        
        foreach(RecipeCard card in recipeCards)
        {
            if (card.recipe != recipeToClose) continue;
            
            cardToClose = card;
            break;
        }

        if (cardToClose)
        {
            cardToClose.CloseCard();
            recipeCards.Remove(cardToClose);
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

    public void GameOverUI(int recipesCompleted, int totalScore)
    {
        timerText.enabled = false;
        recipeWindow.SetActive(false);
        notificationText.enabled = false;
        gameOverWindow.SetActive(true);
        recipesCompletedText.text = "Recipes Completed: " + recipesCompleted;
        scoreText.text = "Final Score: " + totalScore;
    }

    public void RestartLevel()
    {
        AudioManager.instance.PlaySoundClip(uiClip, recipeCardContainer, 1.0f);
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitToMainMenu()
    {
        AudioManager.instance.PlaySoundClip(uiClip, transform, 1.0f);
        SceneManager.LoadSceneAsync(0);
    }
}
