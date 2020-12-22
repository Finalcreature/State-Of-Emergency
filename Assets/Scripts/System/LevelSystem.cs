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
    [SerializeField] int evacuations; // How many need to evactuate
    int evacuated; // How many were evacuated during the session

    [Header("System")]
    LevelLoader levelLoader;
    AIMedic[] medicsOnField; // Count the medics on the field
    List<GameObject> squads = new List<GameObject>(); // A list of medics squad
    List<GameObject> ambulances = new List<GameObject>(); // A list of ambulances on the field

    [Header("End Game Screens")]
    [SerializeField] GameObject endScreen;
    [SerializeField] Text endTitle;
    [SerializeField] Text statistics;
    [Header("")]
    [SerializeField] Button nextLevel;
    [SerializeField] Button retry;
    [SerializeField] Button quit;

    [Header("Choices")]
    [SerializeField] GameObject choices;
    [SerializeField] GameObject medicalKit;
       
    //Delegations
    TimerLogic timerLogic;

    void Start()
    {
        // Get references
        levelLoader = FindObjectOfType<LevelLoader>();
        timerLogic = FindObjectOfType<TimerLogic>();

        //Set difficulty
        SetDifficultyAttributes();

        // Set default settings
        Time.timeScale = 1; // Make sure the game runs at a normal pace
        evacuated = 0;
        obejctive.text = "Evacuated: " + evacuated + "/" + evacuations;

        //Subscribtions
        timerLogic.OnEndTime += EndGame;

    }

    private void SetDifficultyAttributes()
    {
        if (DifficultyPrefs.difficulty == DifficultyPrefs.Difficulies.Easy)
        {
            //Remove additional tools
            Destroy(medicalKit.transform.GetChild(2).gameObject);
            Destroy(medicalKit.transform.GetChild(3).gameObject);

            evacuations = 2;
        }
        else if (DifficultyPrefs.difficulty == DifficultyPrefs.Difficulies.Normal)
        {
            evacuations = 8;
        }
        else if (DifficultyPrefs.difficulty == DifficultyPrefs.Difficulies.Hard)
        {
            evacuations = 15;
        }
        else
        {
            evacuations = 20;
        }
    }

    private void EndGame(object sender, EventArgs e)
    {
        timerLogic.OnEndTime -= EndGame; 
        
        endScreen.SetActive(true);
        statistics.text = "Evacuated " + evacuated + " out of " + evacuations + " injured";
        quit.onClick.AddListener(levelLoader.LoadMainMenu);
        if (evacuated >= evacuations)
        {
            retry.gameObject.SetActive(false);
            nextLevel.onClick.AddListener(levelLoader.LoadNextLevel);
            endTitle.text = "Success";
            endTitle.color = Color.green;
        }
        else
        {
            retry.onClick.AddListener(levelLoader.LoadLevel1);
            endTitle.text = "Fail";
        }
        Time.timeScale = 0; // Stop the game
    }

    public void SetEvacuatedAmount() //called from Attending when the treatment was successful
    {
            evacuated++;
            obejctive.text = "Evacuated: " + evacuated + "/" + evacuations;
    }

    public void ActivateChoices(bool activate) // true when reaching an injured soldier : false when finishing the treatment phase
    {
        choices.SetActive(activate);
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
