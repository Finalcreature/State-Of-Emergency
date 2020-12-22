using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelBehavior : MonoBehaviour
{

    //Options fade in effect
    Color colorAlpha; //Let me control the alpha of the component
    
    void Start()
    {
        colorAlpha = GetComponent<Image>().color;
        colorAlpha.a = 0;
        GetComponent<Image>().color = colorAlpha;
        foreach(Transform child in transform)
        {
         if(child.GetComponent<Image>())
         {
             child.gameObject.SetActive(false);
         }
        }
    }

    void Update()
    {
        if(colorAlpha.a < 1)
        {
            colorAlpha.a += 0.01f; //Gradually increase opacity of panel
            GetComponent<Image>().color = colorAlpha;
        }
        else
        {
            foreach(Transform child in transform)
            {
                if(child.GetComponent<Image>())
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
        
    }
}
