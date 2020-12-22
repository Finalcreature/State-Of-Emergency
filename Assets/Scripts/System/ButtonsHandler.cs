using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{
    // The end game screen buttons stayed in the LevelSystem script for convinience

    [Header("Main Menu")]
    
    [SerializeField] Button startButton;
    [SerializeField] Button optionsButton;
    [SerializeField] Button menuQuitButton;
    [SerializeField] Button menuBriefButton;
    [SerializeField] GameObject breifing;
    [SerializeField] Button briefNextButton;
    int panelIndex;

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
            menuBriefButton.onClick.AddListener(this.ShowBrief);
            briefNextButton.onClick.AddListener(this.NextPanel);
        }
        else if(levelLoader.ActiveScene() == "Options Screen")
        {
            difficultyDropdown.onValueChanged.AddListener(DifficultyPrefs.SetDifficulty);
        }
        else
        {
            stopButton.onClick.AddListener(this.ShowMenu);
        }
        panelIndex = 0;
       
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

    public void ShowBrief()
    {      
        breifing.gameObject.SetActive(true);
        breifing.transform.GetChild(panelIndex).gameObject.SetActive(true);
        panelIndex++;
     }

    // Itirate through the different panels of the briefing
     public void NextPanel()
     {
        for(int index = 0; index < breifing.transform.childCount-1; index++)
        {
            if(panelIndex == 3)
            {
                breifing.gameObject.SetActive(false);
                breifing.transform.GetChild(index).gameObject.SetActive(false);
                panelIndex = -1; // Because the panelIndex is incremented all the time I needed to get -1 so it's basically 0
            }
            else
            {
            if(index == panelIndex)
            {
                breifing.transform.GetChild(index).gameObject.SetActive(true);
               
            }
            else
            {
                breifing.transform.GetChild(index).gameObject.SetActive(false);
            }
            }
        }
             panelIndex++;
     }
}

