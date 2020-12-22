using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{

    //OptionsHandler is only suppose to be present in the options scree, while the SoundController script is moving with the sound preferences between scenes 

    SoundController soundController;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Text[] textsToSet = new Text[4]; // Contain the 4 descriptions of each difficulty for the panel in the options screen
    void Start()
    {
        soundController = FindObjectOfType<SoundController>();
        volumeSlider.onValueChanged.AddListener(soundController.SetSound);   
    }

    public void SetDifficultyText(int chosenText) // In actuality there are 4 panels that turn on and off accordingly
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
