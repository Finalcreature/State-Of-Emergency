using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuredSystem : MonoBehaviour
{
    public int injuredToSpawn;
    [SerializeField] Soldier soldier;
    // Start is called before the first frame update
    void Start()
    {
        injuredToSpawn = 6;
        for (int i = 0; i <= injuredToSpawn; i++)
        {
            Vector2 spawnLocation = new Vector2 (Random.Range(-9,9),Random.Range(-6,6));
            Instantiate(soldier, spawnLocation, Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
