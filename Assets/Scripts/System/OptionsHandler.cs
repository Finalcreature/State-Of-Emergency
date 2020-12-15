using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    SoundController soundController;
    [SerializeField] Slider volumeSlider;
   [SerializeField] Text[] textsToSet = new Text[4];
    void Start()
    {
        soundController = FindObjectOfType<SoundController>();
        volumeSlider.onValueChanged.AddListener(soundController.SetSound);   
    }

    public void SetText(int chosenText)
    {
        for(int textIndex = 0; textIndex < textsToSet.Length; textIndex++)
        {
            if(textIndex == chosenText)
            {
                textsToSet[chosenText].gameObject.SetActive(true);
            }
            else
            {
                textsToSet[textIndex].gameObject.SetActive(false);
            }
        }
        
    }
}
