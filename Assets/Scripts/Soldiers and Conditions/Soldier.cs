using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour
{
    //References
    Player player;
    [SerializeField] GameObject ambulance;
    LevelSystem levelSystem;
    
    //Injuries
    [SerializeField] Sprite[] injuries; //Sprites of injuries
    Sprite currentInjury;
    Color[] conditionsStatus;
    [SerializeField] Sprite deadSoldier;
    float health;
    bool isAlive;
    bool isTreated;
    
    //Ambulance
    Vector2 ambulanceSpawnLocation;
    Vector2 ambulanceParkingLocation;
    GameObject currentAmbulance;
    
    //Medics
    [SerializeField] AIMedic aiMedic;
    Vector2 aiSpawnLocation;
    
    void Start()
    {
        isAlive = true;
        health = 100;
        player = FindObjectOfType<Player>();
        levelSystem = FindObjectOfType<LevelSystem>();
        SetInjury();
        SetRescueTeam();

    }

    void Update()
    {
        if(!levelSystem.GamePaused())
        {
            if(health > 0 && !isTreated)
            {
                health -= Time.deltaTime;
            }
            else if(isAlive && !isTreated)
            {
                SetSoldierAsDead();
            }
        }
    }

    public void SetSoldierAsDead()
    {
        isAlive = false;
        Destroy(transform.GetChild(0).gameObject);
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = deadSoldier;
        levelSystem.DecreaseAmountOfInjured();
    }

    private void SetRescueTeam()
    {
        ambulanceSpawnLocation = new Vector2(-11, -6.8f);
        ambulanceParkingLocation = new Vector2(gameObject.transform.position.x, -6.8f);
        aiSpawnLocation = ambulanceParkingLocation - new Vector2(1, 0);
    }

    private void OnMouseDown()
    {
        {
            if(!player.IsMoving() && isAlive && !levelSystem.GamePaused())
            {
                player.MovePlayer(transform.position);
            }
        }
    }

    void SetInjury()
    {
        currentInjury = injuries[Random.Range(0, 2)];
        conditionsStatus = new Color[] { Color.black, Color.white };
        GameObject conditions = gameObject.transform.GetChild(0).gameObject;
        
        conditions.SetActive(true);

        for (int index = 0; index < 4; index++)
        {
            if (index == 0)
            {
                RandomizeConditionStatus(conditions, index);
            }
            else if (conditions.transform.GetChild(index - 1).GetComponent<SpriteRenderer>().color == Color.black)
            {
                if (index == 4)
                {
                    if ( conditions.transform.GetChild(1).GetComponent<SpriteRenderer>().color == Color.white)
                    {
                        RandomizeConditionStatus(conditions, index);
                    }
                }
                else
                {
                    conditions.transform.GetChild(index).GetComponent<SpriteRenderer>().color = Color.black;
                }
            }
            else
            {
                RandomizeConditionStatus(conditions, index);
            }
        }
        SetHealth(conditions);
    }
    
    private void SetHealth(GameObject conditions)
    {
        Color heart = conditions.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color;
        Color lungs = conditions.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color;
        Color brian = conditions.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().color;
        Color bp = conditions.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().color;

        if (heart == Color.black)
        {
            health = 40;
        }
        else if (lungs == Color.black)
        {
            health = 65;
        }
        else if (brian == Color.black || bp == Color.black)
        {
            health = 85;
        }
    }

    private void RandomizeConditionStatus(GameObject conditions, int index)
    {
        conditions.transform.GetChild(index).GetComponent<SpriteRenderer>().color = conditionsStatus[Random.Range(0, 2)];
    }

    public Sprite GetInjury()
    {
        return currentInjury;
    }

    public void Evacuate()
    {
        isTreated = true;
        levelSystem.DecreaseAmountOfInjured();
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
