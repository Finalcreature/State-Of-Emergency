using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelSystem : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] float setTime;

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
    
     
    void Start()
    {
        player = FindObjectOfType<Player>();
        levelLoader = FindObjectOfType<LevelLoader>();
        isFirstDrop = false;
        isGameStopped = false;
        evacuated = 0;
        obejctive.text = "Evacuated: " + evacuated + "/" + evacuations;
        stopButton.onClick.AddListener(this.ShowMenu);
        minTime = 8;
        maxTime = 20;
        timeToSpawn = Random.Range(minTime,maxTime);
        spawnLocations = new Vector2[] {new Vector2(11,Random.Range(-6,6))};
        Mathf.Round(spawnLocations[0].y);
        numberOfInjured =  0; 
    }

    void Update()
    {
        if(isGameStopped) {return;};
        if(timeToSpawn >=0)
        {
            timeToSpawn -= Time.deltaTime;
        }
        else if(!isFirstDrop || numberOfInjured <=  9)
        {
            Instantiate(InjuredBringer, spawnLocations[0], Quaternion.identity);
            timeToSpawn = Random.Range(minTime,maxTime);
            spawnLocations[0] = new Vector2(11,Random.Range(-6,6));
            soldiers = FindObjectsOfType<Soldier>();
            numberOfInjured = 0;
            foreach (Soldier soldier in soldiers)
            {
                if(soldier.SoldierStatus())
                {
                  numberOfInjured++;
                }
            }
            Debug.Log(numberOfInjured);
            isFirstDrop = true;
        }

        if(setTime >= 0)
        {
            setTime -= Time.deltaTime;
            timer.text = setTime.ToString("F0");
        }
        else if(!isGameStopped)
        {
            EndGame();
        }
        
    }

    public void DecreaseAmountOfInjured()
    {
        numberOfInjured--;
    }

    private void EndGame()
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
