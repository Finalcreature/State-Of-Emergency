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
    bool isGameStopped;
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
    bool isFirstDrop; 
    int numberOfInjured;
    Soldier[] soldiers;
    
    //Delegations
    TimerLogic timerLogic;

    //Events
    public delegate void OnGamePauseArgs();
    public event OnGamePauseArgs OnGamePause;
     
    void Start()
    {
        // Get references
        player = FindObjectOfType<Player>();
        levelLoader = FindObjectOfType<LevelLoader>();
        timerLogic = FindObjectOfType<TimerLogic>();

        // Set default settings
        isGameStopped = false;
        evacuated = 0;
        obejctive.text = "Evacuated: " + evacuated + "/" + evacuations;
        stopButton.onClick.AddListener(this.ShowMenu);

        //Set InjuredBringer
        isFirstDrop = false;
        minTime = 8;
        maxTime = 20;
        timeToSpawn = UnityEngine.Random.Range(minTime,maxTime); /* How often when the InjuredBringer will bring new casulties 
                                                                    (Needed UnityEngine.Random to prevent conflict between it and the System.Random)*/
        spawnLocations = new Vector2[] {new Vector2(11,UnityEngine.Random.Range(-6,6))};
        Mathf.Round(spawnLocations[0].y); //Make sure the injured is dropped in a whole number position to keep logical distances from one injured to the next
        numberOfInjured =  0;

        //Subscribtions
        timerLogic.OnEndTime += EndGame; 
    }

    void Update()
    {
        if(isGameStopped) {OnGamePause?.Invoke();}
        if(timeToSpawn >=0)
        {
            timeToSpawn -= Time.deltaTime;
        }
        else if(!isFirstDrop || numberOfInjured <=  8)
        {
            Instantiate(InjuredBringer, spawnLocations[0], Quaternion.identity);
            timeToSpawn = UnityEngine.Random.Range(minTime,maxTime);
            spawnLocations[0] = new Vector2(11,UnityEngine.Random.Range(-6,6));
            soldiers = FindObjectsOfType<Soldier>();
            numberOfInjured = 0;
            foreach (Soldier soldier in soldiers)
            {
                if(soldier.SoldierStatus())
                {
                  numberOfInjured++;
                }
            }
            isFirstDrop = true;
        }      
    }

    public void DecreaseAmountOfInjured()
    {
        numberOfInjured--;
    }

    private void EndGame(object sender, EventArgs e)
    {
        isGameStopped = true;
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
            isGameStopped = true;
            
        }
        else
        {
            resume.onClick.RemoveAllListeners();
            RemoveQuitListener(pQuit);
            pauseMenu.SetActive(false);
            isGameStopped = false;
        }
        
    }

    void SetRetryButton(Button retry)
    {
        retry.onClick.AddListener(levelLoader.LoadLevel1);
    }

    void SetQuitButton(Button quit)
    {
        quit.onClick.AddListener(levelLoader.LoadMainMenu);
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

    public bool GamePaused()
    {
        return isGameStopped;
    }
}
