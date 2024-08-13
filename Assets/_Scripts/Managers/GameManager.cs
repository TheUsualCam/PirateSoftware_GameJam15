using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game State Parameters")]
    [SerializeField] private float gameTimerInMinutes = 3.0f;

    private UIManager uiManager;
    private RecipeManager recipeManager;

    private float timerMins = 0;
    private float timerSecs = 0;
    private bool isGameOver = false;
    private int totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        recipeManager = FindObjectOfType<RecipeManager>();
        timerMins = gameTimerInMinutes;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            UpdateGameTimer();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.ExitToMainMenu();
        }
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
                uiManager.UpdateTimerUI((int)timerMins, (int)timerSecs);
                GameOver();
            }
        }
        else
        {
            timerSecs -= Time.deltaTime;
        }

        uiManager.UpdateTimerUI((int)timerMins, (int)timerSecs);
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        isGameOver = true;
        uiManager.GameOverUI(recipeManager.GetNumberOfCompletedRecipes(), totalScore);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void UpdateTotalScore(int numIngredientsInRecipe)
    {
        int score = numIngredientsInRecipe * recipeManager.GetNumberOfCompletedRecipes() * 100;
        totalScore += score;
    }
}
