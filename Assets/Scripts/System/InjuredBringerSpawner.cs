using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuredBringerSpawner : MonoBehaviour
{
    InjuredSystem injuredSystem;
    [SerializeField] InjuredBringer InjuredBringer;
    [SerializeField] float timeToSpawn, minTime, maxTime; //TODO set numbers per difficulty
    int maxAmountOfInjuredOnField;
    Vector2[] spawnLocations;
    
    // Start is called before the first frame update
    void Start()
    {
        injuredSystem = FindObjectOfType<InjuredSystem>();
        SetTimesPerDifficulty();
        timeToSpawn = UnityEngine.Random.Range(minTime, maxTime); /* How often when the InjuredBringer will bring new casulties 
                                                                    (Needed UnityEngine.Random to prevent conflict between it and the System.Random)*/
        spawnLocations = new Vector2[] { new Vector2(11, UnityEngine.Random.Range(-6, 6)) };
        Mathf.Round(spawnLocations[0].y); //Make sure the injured is dropped in a whole number position to keep logical distances from one injured to the next

    }

    private void SetTimesPerDifficulty()
    {
        if (DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Easy)
        {
            maxAmountOfInjuredOnField = 3;
            minTime = 30;
            maxTime = 40;
        }
        else if (DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Normal)
        {
            maxAmountOfInjuredOnField = 8;
            minTime = 20;
            maxTime = 30;
        }
        else if (DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Hard)
        {
            maxAmountOfInjuredOnField = 15;
            minTime = 10;
            maxTime = 20;
        }
        else
        {
            maxAmountOfInjuredOnField = 20;
            minTime = 5;
            maxTime = 10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timeToSpawn >=0)
         {
            timeToSpawn -= Time.deltaTime;
         }
            
        else if(injuredSystem.GetAmountOfInjured() <  maxAmountOfInjuredOnField)
        {
            Instantiate(InjuredBringer, spawnLocations[0], Quaternion.identity);
            timeToSpawn = UnityEngine.Random.Range(minTime,maxTime);
            spawnLocations[0] = new Vector2(11,UnityEngine.Random.Range(-6,6));
        }      
        
    }
}
