using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cauldron : MonoBehaviour
{
    [Header("Corruption")]
    [SerializeField] private float currentCorruption = 0f;
    
    [SerializeField]private float corruptionPerShadow = .05f;
    [SerializeField] private float corruptionPerBadIngredient = .05f;
    [SerializeField]private float corruptionPerSecond = 0.01f;
    
    [SerializeField]public float corruptionGraceCooldown = 5f;
    private float corruptionGraceTimeEnd = 0f;
    
    [SerializeField]private Slider corruptionSlider = null;

    private GameManager gameManager;
    private RecipeManager recipeManager;
    private UIManager uiManager;

    // Events
    public static event Action OnCauldronCorrupted;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        recipeManager = FindObjectOfType<RecipeManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if(!gameManager.IsGameOver())
        {
            CorruptionUpdate();
        }
    }

    public void ShadowEntered()
    {
        AddCorruption(corruptionPerShadow);
    }
    
    private void CorruptionUpdate()
    {
        if (Time.time < corruptionGraceTimeEnd)
        {
            return;
        }

        AddCorruption(corruptionPerSecond * Time.deltaTime);
    }
    
    void Corruption()
    {
        currentCorruption = 0.0f;
        corruptionGraceTimeEnd = corruptionGraceCooldown + Time.time;
        recipeManager.LoadNextRecipe();
        uiManager.DisplayNotificationText(false);
        OnCauldronCorrupted?.Invoke();
    }

    public void ResetCorruption()
    {
        currentCorruption = 0.0f;
        corruptionGraceTimeEnd = corruptionGraceCooldown + Time.time;
        UpdateSlider();
        Debug.Log($"Cauldron Reset");
    }
    
    public void AddCorruption(float addition)
    {
        currentCorruption += addition;
        if (currentCorruption >= 1.0f)
        {
            Corruption();
        }

        UpdateSlider();
    }

    private void UpdateSlider()
    {
        corruptionSlider.value = Mathf.Clamp01(currentCorruption);
    }

    public float GetCorruptionPerBadIngredient()
    {
        return corruptionPerBadIngredient;
    }
}
