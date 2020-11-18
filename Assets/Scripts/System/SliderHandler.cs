using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderHandler : MonoBehaviour
{
SoundController soundController;
[SerializeField] Slider volumeSlider;
    void Start()
    {
        soundController = FindObjectOfType<SoundController>();
        volumeSlider.onValueChanged.AddListener(soundController.SetSound); 
    }
}
