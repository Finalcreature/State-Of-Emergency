using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPanelBehavior : MonoBehaviour
{
    Color colorAlpha;
    Transform panelChildrens;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if(colorAlpha.a < 1)
        {
            colorAlpha.a += 0.01f;
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
