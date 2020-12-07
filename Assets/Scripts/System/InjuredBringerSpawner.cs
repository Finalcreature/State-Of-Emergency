﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuredBringerSpawner : MonoBehaviour
{
    LevelSystem levelSystem;
    [SerializeField] InjuredBringer InjuredBringer;
    [SerializeField] float timeToSpawn, minTime, maxTime; //TODO set numbers per difficulty
    Vector2[] spawnLocations;
    bool isFirstDrop; 
    int numberOfInjured;
    bool gamePaused;
    // Start is called before the first frame update
    void Start()
    {
        gamePaused = false;
        levelSystem = FindObjectOfType<LevelSystem>();
        levelSystem.OnGamePause += StopSpawning;
        isFirstDrop = false;
        minTime = 8;
        maxTime = 20;
        timeToSpawn = UnityEngine.Random.Range(minTime,maxTime); /* How often when the InjuredBringer will bring new casulties 
                                                                    (Needed UnityEngine.Random to prevent conflict between it and the System.Random)*/
        spawnLocations = new Vector2[] {new Vector2(11,UnityEngine.Random.Range(-6,6))};
        Mathf.Round(spawnLocations[0].y); //Make sure the injured is dropped in a whole number position to keep logical distances from one injured to the next
        numberOfInjured =  0;
    }

    // Update is called once per frame
    void Update()
    {
        if(!gamePaused)
        {
            Debug.Log("Not paused");
        }
        else
        {
            Debug.Log("Paused");
        }
    }

    void StopSpawning()
    {
        gamePaused = !gamePaused; //flip paused conditions
    }
}