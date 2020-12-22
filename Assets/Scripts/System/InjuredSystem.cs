using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InjuredSystem : MonoBehaviour
{
    [SerializeField] int injuredToSpawn;
    int numberOfInjured;
    [SerializeField] Soldier soldier;
    Vector2[] soldiersLocations;
   
    void Start()
    {
        SpawnInjured();
        numberOfInjured = 0;
    }

    private void SpawnInjured()
    {
        SetAmountOfInjuredToSpawn();
        soldiersLocations = new Vector2[injuredToSpawn]; //TODO bonus - check if a location was already used and change it accordingly
        for (int i = 0; i < soldiersLocations.Length; i++)
        {
            Vector2 spawnLocation = new Vector2(Random.Range(-9, 9), Random.Range(-6, 6));
            Instantiate(soldier, spawnLocation, Quaternion.identity);
            soldiersLocations[i] = spawnLocation;
        }
    }

    private void SetAmountOfInjuredToSpawn()
    {
        if(DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Easy)
        {
            injuredToSpawn = 6;
        }
        else if(DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Normal)
        {
            injuredToSpawn = 9;
        }
        else if(DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Hard)
        {
            injuredToSpawn = 12;
        }
        else
        {
            injuredToSpawn = 15;
        }
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

    public int GetAmountOfInjured()
    {
        return numberOfInjured;
    }
}
