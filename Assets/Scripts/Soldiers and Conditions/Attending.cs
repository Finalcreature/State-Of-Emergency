using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attending : MonoBehaviour
{
    //References
    [SerializeField] GameObject analyzePanel;
    Soldier currentSoldier;
    LevelSystem levelSystem;
    Player player;
    bool tourniquet, bondage;
    bool[] Tools;
    
    //phase
    bool treating;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        levelSystem = FindObjectOfType<LevelSystem>();
        Tools = new bool[] {tourniquet, bondage};
    }
    public void Analyze()
    {
        if(levelSystem.GamePaused()) {return;}
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
        levelSystem.GetButton(2).interactable = true;
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

    public void Tourniquet()
    {
        Tools[0] = true;
        CheckTool();
        Tools[0] = !Tools[0];
    }
        public void Bondage()
    {
        Tools[1] = true;
        CheckTool();
        Tools[1] = !Tools[1];
    }

    void CheckTool()
    {
        if(currentSoldier.GetInjury().name == "Heart" && Tools[0] ||
           currentSoldier.GetInjury().name == "Broken Bone" && Tools[1])
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
        levelSystem.GetButton(2).interactable = false;
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
