using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionsLogic : MonoBehaviour
{
    public Conditions[] conditionsList;
    //Color[] conditionsStatus = new Color[4];

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer[] organsSprites = new SpriteRenderer[4];
        int i = Random.Range(0, 100);
        for (int organ = 0; organ < conditionsList.Length; organ++)
        {
            SpriteRenderer organSprite = transform.GetChild(organ).GetComponent<SpriteRenderer>();
            organsSprites[organ] = organSprite;
            if (i >= conditionsList[organ].minProbability && i <= conditionsList[organ].maxProbability)
            {
                organSprite.color = Color.black;
            }
            else
            {
                organSprite.color = Color.white;
            }
        }

        AdjustConditions(organsSprites);
    }

    private void AdjustConditions(SpriteRenderer[] organsSprites)
    {
        //After assigning the initial status, adjust according to reality
        if (organsSprites[0].color == Color.black)
        {
            for (int organ = 1; organ < organsSprites.Length; organ++)
            {
                organsSprites[organ].color = Color.black;
            }
        }
        else if (organsSprites[1].color == Color.black)
        {
            organsSprites[2].color = Color.black;
        }
        SetHealth(organsSprites);
    }

    public void SetHealth(SpriteRenderer[] conditions)
    {
        int soldierHealth = 100;
        foreach(SpriteRenderer condition in conditions)
        {
            if(condition.color == Color.black)
            {
                soldierHealth -= 20;
            }
        }
        transform.parent.GetComponent<Soldier>().SetSoldierHealth(soldierHealth);
    }
 
    [System.Serializable] //Enables Unity to show this class in the inspector
    public class Conditions  
    {
        [SerializeField] GameObject condition;
        public int minProbability = 0;
        public int maxProbability = 0;
    }
}
