using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InjuredSystem : MonoBehaviour
{
    public int injuredToSpawn;
    [SerializeField] Soldier soldier;
    Vector2[] soldiersLocations = new Vector2[6];
    // Start is called before the first frame update
    void Start()
    {
        SpawnInjured();
    }

    private void SpawnInjured()
    {
        injuredToSpawn = 6;
        for(int i = 0; i < soldiersLocations.Length; i++)
        {
            Vector2 spawnLocation = new Vector2(Random.Range(-9, 9), Random.Range(-6, 6));
            Instantiate(soldier, spawnLocation, Quaternion.identity);
            soldiersLocations[i] = spawnLocation;        
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
