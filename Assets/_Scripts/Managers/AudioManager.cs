using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // [SerializeField] AudioClip[] audioClips;
    [SerializeField] private AudioSource audioSourceObject;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }


    /**
    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "cauldron_complete":
                audioSource.clip = audioClips[0];
                break;
            case "cauldron_erupt":
                audioSource.clip = audioClips[1];
                break;
            case "cauldron_item_correct":
                audioSource.clip = audioClips[2];
                break;
            case "cauldron_item_incorrect":
                audioSource.clip = audioClips[3];
                break;

            case "item_drop":
                audioSource.clip = audioClips[4];
                break;
            case "item_pickup":
                audioSource.clip = audioClips[5];
                break;
            case "item_spawn":
                audioSource.clip = audioClips[6];
                break;
            case "item_throw":
                audioSource.clip = audioClips[7];
                break;

            case "portal_activate":
                audioSource.clip = audioClips[8];
                break;
            case "portal_deactivate":
                audioSource.clip = audioClips[9];
                break;

            case "shadow_attack":
                audioSource.clip = audioClips[10];
                break;
            case "shadow_clear":
                audioSource.clip = audioClips[11];
                break;

            case "ui":
                audioSource.clip = audioClips[12];
                break;
        }
    }
    */


    public void PlaySoundClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn gameObject
        AudioSource audioSource = Instantiate(audioSourceObject, spawnTransform.position, Quaternion.identity);

        // Assign the audioClip
        audioSource.clip = audioClip;

        // Assign volume
        audioSource.volume = volume;

        // Play sound
        audioSource.Play();

        // Get length of sound
        float clipLength = audioSource.clip.length;

        // Destroy clip after it has played
        Destroy(audioSource.gameObject, clipLength);
    }
}
