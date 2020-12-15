using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    private void Awake()
    {
        if(FindObjectsOfType<LevelLoader>().Length > 1)
        {
            Destroy(gameObject);
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

    //No real level so it's just a pseudo-function
     public void LoadNextLevel()
     {
         Debug.Log("Level 2 loaded");
     }


     
}
