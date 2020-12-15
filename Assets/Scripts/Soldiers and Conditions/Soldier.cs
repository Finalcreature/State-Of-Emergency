using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour //Should have named it Injured
{
    //References
    Player player;
    [SerializeField] GameObject ambulance;
    LevelSystem levelSystem;
    InjuredSystem injuredSystem;
    [SerializeField] ConditionsLogic conditionsLogic;
    
    //Injuries
    [SerializeField] Sprite[] injuries; //Sprites of injuries
    Sprite currentInjury;
    [SerializeField] Sprite deadSoldier; //Sprite when the soldier dies
    float health, currentHealth;
    bool isAlive;
    bool isTreated;
    
    //Ambulance
    Vector2 ambulanceSpawnLocation; //TODO add another spawning location
    Vector2 ambulanceParkingLocation;
    GameObject currentAmbulance;
    
    //Medics
    [SerializeField] AIMedic aiMedic;
    Vector2 aiSpawnLocation;

    //Sounds
    [SerializeField] AudioClip dying;
    
    void Start()
    {
        //Default conditions
        isAlive = true;
        SetHealthPerDifficult();


        //Get references to scripts
        player = FindObjectOfType<Player>();
        levelSystem = FindObjectOfType<LevelSystem>();
        injuredSystem = FindObjectOfType<InjuredSystem>();

        //Set amount of soldiers
        injuredSystem.SetAmountOfInjured(isAlive);

        //Set conditions and designated ambulance - TODO check OneNote
        SetInjury();
        SetRescueTeam();
    }

    private void SetHealthPerDifficult()
    {
        if (DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Easy)
        {
            health = 300;
        }
        else if (DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Normal)
        {
            health = 200;
        }
        else
        {
            health = 100;
        }
    }

    //Decreases injured health
    void Update() 
    {
            currentHealth = health;
            if(health > 0 && !isTreated)
            {
                health -= Time.deltaTime;
            }
            else if(isAlive && !isTreated)
            {
                SetSoldierAsDead();
            }
    }

    public void SetSoldierAsDead()
    {
        isAlive = false;
        Destroy(transform.GetChild(0).gameObject); // Remove the conditions panel
        GetComponent<Animator>().enabled = false; //No death animation
        GetComponent<SpriteRenderer>().sprite = deadSoldier;
        injuredSystem.SetAmountOfInjured(isAlive); //decrease the amout of injured
        GetComponent<AudioSource>().PlayOneShot(dying);
    }

    //Every soldier has a dedicated rescue team
    private void SetRescueTeam()
    {
        ambulanceSpawnLocation = new Vector2(-11, -6.8f);
        ambulanceParkingLocation = new Vector2(gameObject.transform.position.x, -6.8f);
        aiSpawnLocation = ambulanceParkingLocation - new Vector2(1, 0);
    }

    private void OnMouseDown()
    {
        {
            if(!player.IsMoving() && isAlive && Time.timeScale != 0)
            {
                player.MovePlayer(transform.position);
            }
        }
    }

    void SetInjury()
    {
        if(DifficultyPrefs.GetDifficuly() == DifficultyPrefs.Difficulies.Easy)
        {
            currentInjury = injuries[Random.Range(0, 2)];
        }
        else
        {
            currentInjury = injuries[Random.Range(0,4)];
        }
        ConditionsLogic attachedCoditions =  Instantiate(conditionsLogic,transform.position + new Vector3(-0.1f,0.5f),transform.rotation) as ConditionsLogic;
        attachedCoditions.transform.parent = transform;
    }

    public void SetSoldierHealth(int healthToSet)
    {
        health = healthToSet;
    }

    public Sprite GetInjury()
    {
        return currentInjury;
    }

    public void Evacuate()
    {
        isTreated = true;
        injuredSystem.SetAmountOfInjured(!isAlive);
        GameObject thisAmbulance =  Instantiate(ambulance, ambulanceSpawnLocation, Quaternion.identity) as GameObject;
        levelSystem.AddAmbulance(thisAmbulance);
        currentAmbulance = levelSystem.GetAmbulances()[levelSystem.GetAmbulances().LastIndexOf(thisAmbulance)];
        StartCoroutine(MoveAmbulance(thisAmbulance));
    }

    IEnumerator MoveAmbulance(GameObject thisAmbulance)
    {
        while( thisAmbulance.transform.position.x != ambulanceParkingLocation.x)
        {   
            yield return thisAmbulance ;
            thisAmbulance.transform.position = Vector3.MoveTowards(thisAmbulance.transform.position,ambulanceParkingLocation,Time.deltaTime * 3);
        }
        StartCoroutine(SpawnMedic());
        
    }

    IEnumerator SpawnMedic()
    {
         for (int i = 0; i < 2; i++)
            {
                AIMedic newAIMedic = Instantiate(aiMedic, aiSpawnLocation, Quaternion.identity) as AIMedic;
                newAIMedic.name = "Medic " + i;
                yield return new WaitForSeconds(0.1f);
                newAIMedic.Evacuate(transform.position, this, currentAmbulance);
            }    
    }

    public Vector3 AmbulancePos()
    {
        return aiSpawnLocation;
    }

    public void Reached(GameObject thisSquad)
    {
        
        foreach(Transform medic in thisSquad.transform)
        {
            medic.GetComponent<AIMedic>().GoingBack(this);
        }
    }

    public void Evacuated(GameObject thisSquad)
    {
        foreach(Transform medic in thisSquad.transform)
        {
            if(medic.transform.position.y < -5)
            {        
                Destroy(medic.gameObject);
            }
        }
        StopAllCoroutines();
        gameObject.transform.parent = currentAmbulance.transform;
        StartCoroutine(ExitAmbulance());
    }

    IEnumerator ExitAmbulance()
    {
        while(true)
        {
            currentAmbulance.transform.position = Vector3.MoveTowards(currentAmbulance.transform.position,new Vector2(12,currentAmbulance.transform.position.y),Time.deltaTime * 3);
            yield return null;
            if(currentAmbulance.transform.position.x == 12)
            {
                Destroy(currentAmbulance);
            }
        }
    }

    public bool SoldierStatus()
    {
        return isAlive;
    }

}
