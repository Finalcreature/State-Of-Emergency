using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMedic : MonoBehaviour
{ 
    Vector3 offset; // The location the medic needs to reach relatevily to the injured
    bool moving;
    LevelSystem levelSystem;
    [SerializeField] GameObject squad;
    GameObject currentSquad;
    [SerializeField] Sprite pickupPose;
  
    private void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();
        SetSquad();
        moving = true;
    }

    private void SetSquad()
    {
        /*When the medic is being instantiated from the Soldier - the medic check if he is the first one in the squad by checking if there is a medic with no pair.
                  If there is no single medic then the medic that was created will instantiate a sqaud and make it its own parent */
        if (levelSystem.GetAllMedics().Length % 2 != 0) //Check if there is a medic with no pair
        {
            GameObject medicSquad = Instantiate(squad, transform.position, Quaternion.identity) as GameObject;
            gameObject.transform.parent = medicSquad.transform; //Set this.medic as the child of the intantiated squad
            levelSystem.AddASquads(medicSquad); //Add the squad to the list of squads
            currentSquad = levelSystem.GetSquads()[levelSystem.GetSquads().LastIndexOf(medicSquad)]; //Set the current squad to the last squad that was added to the list of squads
            offset = new Vector3(0.3f, 0.1f);
        }
        else
        {
            currentSquad = levelSystem.GetSquads()[levelSystem.GetSquads().Count - 1];
            gameObject.transform.parent = currentSquad.transform;
            offset = new Vector3(-0.3f, 0.1f);
        }
    }

    public void Evacuate(Vector3 soldierPos, Soldier soldier, GameObject ambulance)
    {
        currentSquad.transform.parent = ambulance.transform; 
        StartCoroutine(Moving(soldierPos, soldier));
    }

        IEnumerator Moving(Vector3 soliderPosition, Soldier soldier) //move the medic squad to the injured
        {
            while(true)
            {    
                if(moving)
                {
                    transform.position = Vector3.MoveTowards(transform.position,soliderPosition + offset,Time.deltaTime);
              
                    if(currentSquad.transform.childCount == 2)
                    {
                        if(currentSquad.transform.GetChild(0).transform.position.y == currentSquad.transform.GetChild(1).transform.position.y )// Check if both medics of the squad are aligned with the injured
                        {
                            moving = false;
                            soldier.Reached(currentSquad);
                        }
                    }
                }
                yield return null;  
            }
        }


        public void GoingBack(Soldier soldier)
        {
            StartCoroutine(BackToAmbulance(soldier));  
        }

        IEnumerator BackToAmbulance(Soldier thisSoldier)
        {
            while(true)
            {  
                foreach(Transform medic in currentSquad.transform)
                {
                    medic.GetComponent<SpriteRenderer>().sprite = pickupPose;
                    if(medic.name == "Medic 0")
                    {
                        medic.GetComponent<SpriteRenderer>().flipX = true;
                        medic.transform.position = thisSoldier.transform.position + offset;
                    }
                    else
                    {
                        medic.transform.position = thisSoldier.transform.position - offset; 
                    }
                }
                thisSoldier.transform.position = Vector3.MoveTowards(thisSoldier.transform.position, thisSoldier.AmbulancePos(), Time.deltaTime);
                yield return null;
                if(thisSoldier.transform.position == thisSoldier.AmbulancePos() && name == "Medic 0")
                {
                    thisSoldier.Evacuated(currentSquad);
                    StopAllCoroutines();
                }
            }     
        } 
}
