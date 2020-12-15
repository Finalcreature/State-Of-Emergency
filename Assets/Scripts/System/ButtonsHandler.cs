using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ButtonsHandler : MonoBehaviour
{
    [Header("Main Menu")]
    
    [SerializeField] Button startButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button menuQuitButton;

    [Header("Pause Menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button stopButton;
    [SerializeField] Button resumeButton;
    [SerializeField] Button lvlQuitButton;

    [Header("Options Menu")]
    [SerializeField] Dropdown difficultyDropdown;

    [Header("Choices")]
    [SerializeField] Button treat;
    LevelLoader levelLoader;

    void Start()
    {  
        levelLoader = FindObjectOfType<LevelLoader>();
        if(levelLoader.ActiveScene() == "Main Menu")
        {
            startButton.onClick.AddListener(levelLoader.LoadLevel1);
            optionsButton.onClick.AddListener(levelLoader.LoadOptions);
            menuQuitButton.onClick.AddListener(levelLoader.QuiteGame);
        }
        else if(levelLoader.ActiveScene() == "Options Screen")
        {
            difficultyDropdown.onValueChanged.AddListener(DifficultyPrefs.SetDifficulty);
        }
        else
        {
            stopButton.onClick.AddListener(this.ShowMenu);
        }
       
    }

    public void ShowMenu()
    {
        if(!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            resumeButton.onClick.AddListener(this.ShowMenu);
            lvlQuitButton.onClick.AddListener(levelLoader.LoadMainMenu);
            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);
            resumeButton.onClick.RemoveAllListeners();
            lvlQuitButton.onClick.RemoveAllListeners();
            Time.timeScale = 1;
        }

    }
    public Button GetTreatOrPause(bool treatButton)
    {
        if(treatButton)
        {
            return treat;
        }
        else
        {
            return stopButton;
        }
    }
}
