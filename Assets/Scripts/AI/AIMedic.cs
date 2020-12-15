using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMedic : MonoBehaviour
{ 
    Vector3 offset;
    bool moving;
    LevelSystem levelSystem;
    [SerializeField] GameObject squad;
    GameObject currentSquad;
    [SerializeField] Sprite pickupPose;
  
   
    
    private void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();
        
        if(levelSystem.GetAllMedics().Length  % 2 != 0)
        {
            GameObject medicSquad = Instantiate(squad,transform.position,Quaternion.identity) as GameObject;
            gameObject.transform.parent = medicSquad.transform;
            levelSystem.AddASquads(medicSquad);
            currentSquad = levelSystem.GetSquads()[levelSystem.GetSquads().LastIndexOf(medicSquad)];
            offset = new Vector3(0.3f,0.1f);
        }
        else if(levelSystem.GetAllMedics().Length > 1)
        {   
            currentSquad = levelSystem.GetSquads()[levelSystem.GetSquads().Count-1]; 
            gameObject.transform.parent = currentSquad.transform;
            offset = new Vector3(-0.3f,0.1f);
        }
        moving = true;
    }
    public void Evacuate(Vector3 soldierPos, Soldier soldier, GameObject ambulance)
    {
        currentSquad.transform.parent = ambulance.transform;
        StartCoroutine(Moving(soldierPos, soldier));
    }

        IEnumerator Moving(Vector3 soliderPosition, Soldier soldier)
        {
            while(moving)
            {    
                if(moving)
                {
                    transform.position = Vector3.MoveTowards(transform.position,soliderPosition + offset,Time.deltaTime);
              
                    if(currentSquad.transform.childCount == 2)
                    {
                        if(currentSquad.transform.GetChild(0).transform.position.y == currentSquad.transform.GetChild(1).transform.position.y )
                        {
                            moving = false;
                            soldier.Reached(currentSquad);
                        }
                    }
                }
                yield return moving;  
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

        public bool movingStatus()
        {
            return moving;
        }

        
}
