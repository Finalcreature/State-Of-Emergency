using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
//PlayerMovement
bool isMoving; 

//Player Animation
Animator playerAnim;

//Refernces
LevelSystem levelSystem;
ButtonsHandler buttonsHandler;
Attending attending;

//Camera
Camera gameCam;
Vector3 camStartPos;
float camStartZoom, zoomTarget;


	void Start()
    {
        //Get references to scripts
        levelSystem = FindObjectOfType<LevelSystem>();
        playerAnim = GetComponent<Animator>();
        buttonsHandler = FindObjectOfType<ButtonsHandler>();

        //Set camera and player movement condition
        isMoving = false;
        SetCamera(); 
        
    }

    //Set all camera related variables
    private void SetCamera()
    {
        gameCam = Camera.main;
        camStartPos = gameCam.transform.position;
        camStartZoom = gameCam.orthographicSize;
        zoomTarget = 1.35f; //Camera orthographic size when zoomed in
    }



    public void MovePlayer(Vector3 soliderPos)
    {
        levelSystem.ActivateChoices(false);
        isMoving = true;
        if(isMoving)
        {
           StartCoroutine(Moving(soliderPos));
        }
        else
        {      
            isMoving = false;
        }      
    }

    IEnumerator Moving(Vector3 soliderPosition)
    {
        while (isMoving)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, soliderPosition, Time.deltaTime);

            if (soliderPosition.y >= transform.position.y)
            {
                playerAnim.SetTrigger("goUp");
            }
            else
            {
                playerAnim.SetTrigger("goDown");
            }

            if (transform.position.x == soliderPosition.x && transform.position.y == soliderPosition.y)
            {
                TreatingPhase();
            }

        }
        ResetAnims();
    }

    private void ResetAnims()
    {
        playerAnim.ResetTrigger("goUp");
        playerAnim.ResetTrigger("goDown");
        playerAnim.SetTrigger("stand");
    }

    void TreatingPhase()
    {
        levelSystem.ActivateChoices(true);
        attending = FindObjectOfType<Attending>();
        attending.SetTreating(true);
        StartCoroutine(ZoomIn());
        isMoving = false;
    }

    IEnumerator ZoomIn()
    {
        while(attending.isTreating()) 
        {
            yield return  null;
            gameCam.orthographicSize = Mathf.Lerp(gameCam.orthographicSize, zoomTarget, Time.deltaTime * 2); 
                gameCam.transform.position = Vector3.Lerp
                (
                    gameCam.transform.position,
                    new Vector3(transform.position.x, transform.position.y, -10),
                    Time.deltaTime * 2
                ); 
                buttonsHandler.GetTreatOrPause(false).gameObject.SetActive(false); // Set the stop button to false - prevent player from pausing the game during treating phase;
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public void ZoomOut()
    {
       StartCoroutine(ZoomingOut());
    } 

    IEnumerator ZoomingOut()
    {
        while( !attending.isTreating() &&  Camera.main.orthographicSize < camStartZoom - 0.2f) // The camera never returns to it's starting pos hence the -0.2f
        {
            yield return  null;
            gameCam.orthographicSize = Mathf.Lerp(gameCam.orthographicSize, camStartZoom, Time.deltaTime * 2); 
            gameCam.transform.position = Vector3.Lerp(gameCam.transform.position, camStartPos, Time.deltaTime * 2);
            buttonsHandler.GetTreatOrPause(false).gameObject.SetActive(true); // Set the stop button to true - player can pause now if desire
        }
    }  

}



