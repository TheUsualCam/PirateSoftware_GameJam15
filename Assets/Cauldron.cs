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
    [SerializeField]private float corruptionPerSecond = 0.01f;
    
    [SerializeField]public float corruptionGraceCooldown = 5f;
    private float corruptionGraceTimeEnd = 0f;
    
    [SerializeField]private Slider corruptionSlider = null;

    // Events
    public static event Action OnCauldronCorrupted;

    void Update()
    {
        CorruptionUpdate();
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
        // Spawn Shadows
        // Generate a new recipe + Ingredients
        OnCauldronCorrupted?.Invoke();
        Debug.Log($"Cauldron Corrupted");
    }
    
    void AddCorruption(float addition)
    {
        currentCorruption += addition;
        if (currentCorruption >= 1.0f)
        {
            Corruption();
        }

        corruptionSlider.value = Mathf.Clamp01(currentCorruption);
    }
}
