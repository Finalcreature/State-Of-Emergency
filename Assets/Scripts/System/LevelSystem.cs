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
    AIMedic[] medicsOnField;
    List<GameObject> squads = new List<GameObject>();
    List<GameObject> ambulances = new List<GameObject>();

    [Header("End Game Screens")]
    [SerializeField] GameObject endScreen;
    [SerializeField] Text endTitle;
    [SerializeField] Text statistics;
    [SerializeField] Button[] endScreenButtons; //{nextLevel, retry, quit}

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

        // Set default settings
        Time.timeScale = 1;
        evacuated = 0;
        obejctive.text = "Evacuated: " + evacuated + "/" + evacuations;

        //Set difficulty
        if(DifficultyPrefs.difficulty == DifficultyPrefs.Difficulies.Easy)
        {
            Destroy(medicalKit.transform.GetChild(2).gameObject);
            Destroy(medicalKit.transform.GetChild(3).gameObject);
        }
        
        //Subscribtions
        timerLogic.OnEndTime += EndGame; 
        
    }

    private void EndGame(object sender, EventArgs e)
    {
        timerLogic.OnEndTime -= EndGame; 
        
        endScreen.SetActive(true);
        statistics.text = "Evacuated " + evacuated + " out of " + evacuations + " injured";
        endScreenButtons[2].onClick.AddListener(levelLoader.LoadMainMenu);
        if (evacuated >= evacuations)
        {
            endScreenButtons[1].gameObject.SetActive(false);
            endScreenButtons[0].onClick.AddListener(levelLoader.LoadNextLevel);
            Debug.Log(endScreenButtons[0].name);
            endTitle.text = "Success";
            endTitle.color = Color.green;
        }
        else
        {
            endScreenButtons[1].onClick.AddListener(levelLoader.LoadLevel1);
            endTitle.text = "fail";
        }
        Time.timeScale = 0;
    }

    public void SetEvacuatedAmount()
    {
            evacuated++;
            obejctive.text = "Evacuated: " + evacuated + "/" + evacuations;
    }

    public void ActivateChoices(bool activate)
    {
        choices.SetActive(activate);
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
