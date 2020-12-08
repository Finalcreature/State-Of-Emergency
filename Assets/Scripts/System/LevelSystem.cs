using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class LevelSystem : MonoBehaviour
{
    [Header("Objective")]
    [SerializeField] TextMeshProUGUI obejctive;
    [SerializeField] int evacuations;
    int evacuated;

    [Header("System")]
    LevelLoader levelLoader;
    Player player;
    AIMedic[] medicsOnField;
    List<GameObject> squads = new List<GameObject>();
    List<GameObject> ambulances = new List<GameObject>();
    

    [Header("Pause Menu")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Button stopButton;
    [SerializeField] Button resume;
    [SerializeField] Button pQuit;

    [Header("End Game Screens")]
    [SerializeField] GameObject screenStates;
    [SerializeField] Button fRetry;
    [SerializeField] Button fQuit;

    [Header("Choices")]
    [SerializeField] GameObject choices;
    [SerializeField] GameObject medicalKit;
    [SerializeField] Button treat;

    [Header("Level Manager")]
    [SerializeField] InjuredBringer InjuredBringer;
    [SerializeField] float timeToSpawn, minTime, maxTime;
    Vector2[] spawnLocations;
    int numberOfInjured;
    Soldier[] soldiers;
    
    //Delegations
    TimerLogic timerLogic;

    void Start()
    {
        // Get references
        player = FindObjectOfType<Player>();
        levelLoader = FindObjectOfType<LevelLoader>();
        timerLogic = FindObjectOfType<TimerLogic>();

        // Set default settings
        evacuated = 0;
        obejctive.text = "Evacuated: " + evacuated + "/" + evacuations;
        stopButton.onClick.AddListener(this.ShowMenu);
        numberOfInjured = 0;

        //Subscribtions
        timerLogic.OnEndTime += EndGame; 
    }

    public void SetAmountOfInjured(bool isAlive)
    {
        if(isAlive)
        {
            numberOfInjured++;    
        }
        else
        {
            numberOfInjured--;
        }
    }

    private void EndGame(object sender, EventArgs e)
    {
        screenStates.SetActive(true);
        if (evacuated >= evacuations)
        {
            SuccessScreen();
        }
        else
        {
            FailScreen();
        }
    }

    private void FailScreen()
    {
        GameObject failScreen = screenStates.transform.GetChild(0).gameObject;
        failScreen.SetActive(true);
        Text failScreenText = failScreen.transform.GetChild(1).GetComponent<Text>();
        failScreenText.text = "Evacuated only " + evacuated + " out of " + evacuations + " injured";
        SetQuitButton(fQuit);
        SetRetryButton(fRetry);
    }

    private void SuccessScreen()
    {
        GameObject successScreen = screenStates.transform.GetChild(1).gameObject;
        successScreen.SetActive(true);
        Text successScreenText = successScreen.transform.GetChild(1).GetComponent<Text>();
        successScreenText.text = "Evacuated only " + evacuated + " out of " + evacuations + " injured";
    }

    public void SetEvacuatedAmount()
    {
            evacuated++;
            obejctive.text = "Evacuated: " + evacuated + "/" + evacuations;
    }

    public void ShowMenu() //TODO  stop AI
    {
        if(!pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(true);
            resume.onClick.AddListener(this.ShowMenu);
            SetQuitButton(pQuit);
            Time.timeScale = 0;
            
        }
        else
        {
            resume.onClick.RemoveAllListeners();
            RemoveQuitListener(pQuit);
            pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        
    }

    void SetRetryButton(Button retry)
    {
        retry.onClick.AddListener(levelLoader.LoadLevel1);
    }

    void SetQuitButton(Button quit)
    {
        //quit.onClick.AddListener(levelLoader.LoadMainMenu); //TODO re-enable after debugging
    }
    void RemoveQuitListener(Button quit)
    {
        quit.onClick.RemoveAllListeners();
    }
    public void ActivateChoices(bool activate)
    {
        choices.SetActive(activate);
    }

    public Button GetButton(int index)
    {
        Button[] buttons = {resume, pQuit, treat, stopButton};
        return buttons[index];
    }

    public GameObject GetKit()
    {
        return medicalKit;
    }

    public AIMedic[] GetAllMedics()
    {
        medicsOnField = FindObjectsOfType<AIMedic>();
        return medicsOnField;
    }

    public void AddASquads(GameObject newSquad)
    {   
        
        squads.Add(newSquad);
    }
    public List<GameObject> GetSquads()
    {
        return squads;
    }
    public void AddAmbulance(GameObject newAmbulance)
    {
        ambulances.Add(newAmbulance);
    }

    public List<GameObject> GetAmbulances()
    {
        return ambulances;
    }
}
