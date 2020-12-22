using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    //Refernce
    [SerializeField] AudioMixer audioMixer;

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
        audioMixer.SetFloat("masterVolume", volume); //set the volume = Used by the Audio Mixer
    }
}
