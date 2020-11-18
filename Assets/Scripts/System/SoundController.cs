using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    //Refernces
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider volumeSlider;

    private void Awake() 
    {
        if(FindObjectsOfType<SoundController>().Length > 1)
        {
            Destroy(gameObject);
        }    
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void SetSound(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }
}
