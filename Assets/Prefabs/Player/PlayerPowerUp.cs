using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    [SerializeField] private bool isActive;
    [SerializeField] private float duration;
    [SerializeField] private ParticleSystem[] powerUpParticles;
    private float _finishTime;

    [SerializeField] private AnimationCurve speedMultiplierCurve;
    [SerializeField] private float throwMultiplier;

    public AudioClip powerupStart;

    public void StartPowerUp()
    {
        isActive = true;
        _finishTime = Time.time + duration;
        AudioManager.instance.PlaySoundClip(powerupStart, this.GetComponentInParent<Transform>(), 1f);
        GetComponent<AudioSource>().volume = 0.6f;

        foreach (var particleSystem in powerUpParticles)
        {
            particleSystem.Play();
        }
    }

    private void EndPowerUp()
    {
        isActive = false;
        _finishTime = 0;
        
        foreach (var particleSystem in powerUpParticles)
        {
            particleSystem.Stop();
        }

        GetComponent<AudioSource>().volume = 0f;
    }

    private void Update()
    {
        if (isActive && Time.time >= _finishTime)
        {
            EndPowerUp();
        }
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (!isActive)
        {
            return;
        }
        ShadowCreature shadow = other.gameObject.GetComponent<ShadowCreature>();

        if (shadow)
        {
            Destroy(shadow.gameObject);
        }
    }

    public float GetPowerUpSpeedMultiplier()
    {
        float multiplierAtTime = speedMultiplierCurve.Evaluate((_finishTime - Time.time) / duration);
        return isActive ? multiplierAtTime : 1f;
    }
}
