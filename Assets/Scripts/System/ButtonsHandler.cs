using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{
    [SerializeField] Button start, options, quit;
    LevelLoader levelLoader;

    void Start()
    {  
        levelLoader = FindObjectOfType<LevelLoader>();
        start.onClick.AddListener(levelLoader.LoadLevel1);
        options.onClick.AddListener(levelLoader.LoadOptions);
        quit.onClick.AddListener(levelLoader.QuiteGame);
    }
}
