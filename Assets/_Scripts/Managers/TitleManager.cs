using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public Transform audioPosition;
    public GameObject mainMenuPanel;
    public GameObject[] tutorialPanels;

    private AudioSource audioSource;

    private void Start()
    {
        foreach (GameObject panel in tutorialPanels)
        {
            panel.SetActive(false);
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        audioSource.Play();
        SceneManager.LoadSceneAsync(1);
    }

    public void OpenTutorialPanel()
    {
        audioSource.Play();
        tutorialPanels[0].SetActive(true);
    }

    public void CloseTutorialPanels()
    {
        audioSource.Play();
        foreach (GameObject panel in tutorialPanels)
        {
            panel.SetActive(false);
        }
    }

    public void NextTutorialPanel()
    {
        audioSource.Play();
        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            if(tutorialPanels[i].activeInHierarchy)
            {
                tutorialPanels[i].SetActive(false);
                tutorialPanels[i + 1].SetActive(true);
                return;
            }
        }
    }

    public void PreviousTutorialPanel()
    {
        audioSource.Play();
        for (int i = 0; i < tutorialPanels.Length;i++)
        {
            if(tutorialPanels[i].activeInHierarchy)
            {
                tutorialPanels[i].SetActive(false);
                tutorialPanels[i - 1].SetActive(true);
            }
        }
    }

    public void ExitGame()
    {
        audioSource.Play();
        Application.Quit();
    }
}
