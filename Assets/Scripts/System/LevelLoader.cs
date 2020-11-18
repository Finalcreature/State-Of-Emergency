using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private void Awake() {
        LevelLoader[] levelLoaders = FindObjectsOfType<LevelLoader>();
        foreach(LevelLoader ll in levelLoaders)
        {
            Debug.Log(ll.GetInstanceID());
        }
        if(FindObjectsOfType<LevelLoader>().Length > 1)
        {
            Destroy(levelLoaders[1]);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
         {
            LoadMainMenu();
         }
    }

    public void QuiteGame()
    {
        Application.Quit();
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options Screen");
    }

     public void LoadLevel1()
     {
         SceneManager.LoadScene(1);
        
     }

     public void LoadMainMenu()
     {
        SceneManager.LoadScene(0); 
     }

     public string ActiveScene()
     {
         return SceneManager.GetActiveScene().name;
     }

     
}
