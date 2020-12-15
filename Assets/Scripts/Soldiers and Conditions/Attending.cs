using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attending : MonoBehaviour
{
    //References
    [SerializeField] GameObject analyzePanel;
    Soldier currentSoldier;
    ButtonsHandler buttonsHandler;
    LevelSystem levelSystem;
    Player player;

    //phase
    bool treating;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        levelSystem = FindObjectOfType<LevelSystem>();
        buttonsHandler = FindObjectOfType<ButtonsHandler>();
    }
    public void Analyze()
    {
        Soldier[] soldiers = FindObjectsOfType<Soldier>();
        foreach (Soldier soldier in soldiers)
        {
            if(soldier.transform.position == player.transform.position)
            {
                currentSoldier = soldier;
                currentSoldier.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        analyzePanel.SetActive(true);
        buttonsHandler.GetTreatOrPause(true).interactable = true; //Enable treat button
        SetTreating(true);
        analyzePanel.GetComponent<Image>().sprite =  currentSoldier.GetInjury();
    }

    public void Treat()
    {   	
	    foreach(Transform child in levelSystem.GetKit().transform)
	    { 
		    child.gameObject.SetActive(true);
        }       
    }

    public void CheckTool(string treatingTool)
    {
        if(treatingTool == currentSoldier.GetInjury().name)
        {
            currentSoldier.Evacuate();
            levelSystem.SetEvacuatedAmount();
        }
        else
        {
            currentSoldier.SetSoldierAsDead();
        }
        FinishTreating();
    }

    void FinishTreating()
    {
        SetTreating(false);
        player.ZoomOut();
        analyzePanel.SetActive(false);  
        buttonsHandler.GetTreatOrPause(true).interactable = false; 
        foreach(Transform child in levelSystem.GetKit().transform)
	    { 
		    child.gameObject.SetActive(false);
        }
        gameObject.SetActive(false);    
    }

    public bool isTreating()
    {
        return treating;
    }

    public void SetTreating(bool treat)
    {
        treating = treat;
    }
}
