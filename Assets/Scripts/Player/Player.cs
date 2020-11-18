using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
//PlayerMovement
float mouseLocationX, mouseLocationY;
bool move;

//Refernces
LevelSystem levelSystem;
Attending attending;

//Camera
Camera gameCam;
Vector3 camStartPos;
float camStartZoom, zoomTarget;


	void Start()
    {
        levelSystem = FindObjectOfType<LevelSystem>();
         SetCamera();
         SetPlayer();
    }

    private void SetPlayer()
    {
        mouseLocationX = Input.mousePosition.x;
        mouseLocationY = Input.mousePosition.y;
        move = false;
    }

    private void SetCamera()
    {
        gameCam = Camera.main;
        camStartPos = gameCam.transform.position;
        camStartZoom = gameCam.orthographicSize;
        zoomTarget = 1.35f;
    }

    Vector2 SetMousePosition()
	    {
	        Vector2 clickPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
	        Vector2 worldPos = gameCam.ScreenToWorldPoint(clickPos);
	        return worldPos;
        }

    public void MovePlayer(Vector3 soliderPos)
    {
        levelSystem.ActivateChoices(false);
        move = true;
        if(move)
        {
           StartCoroutine(Moving(soliderPos));
        }
        else
        {      
            move = false;
        }
            
    }

    IEnumerator Moving(Vector3 soliderPosition)
    {
        while(move)
        {    
            yield return move;
            transform.position = Vector3.MoveTowards(transform.position,soliderPosition,Time.deltaTime);
          
            if(transform.position.x == soliderPosition.x && transform.position.y == soliderPosition.y)
            {
                TreatingPhase();
            }

        }
    }

    void TreatingPhase()
    {
        levelSystem.ActivateChoices(true);
        attending = FindObjectOfType<Attending>();
        attending.SetTreating(true);
        StartCoroutine(ZoomIn());
        move = false;
        }

    IEnumerator ZoomIn()
    {
        while(attending.isTreating()) 
        {
            yield return  Camera.main.orthographicSize > zoomTarget;
            gameCam.orthographicSize = Mathf.Lerp(gameCam.orthographicSize, zoomTarget, Time.deltaTime * 2); 
                gameCam.transform.position = Vector3.Lerp
                (
                    gameCam.transform.position,
                    new Vector3(transform.position.x, transform.position.y, -10),
                    Time.deltaTime * 2
                ); 
                levelSystem.GetButton(3).gameObject.SetActive(false);
        }
    }

    public bool IsMoving()
    {
        return move;
    }

    public void ZoomOut()
    {
       StartCoroutine(ZoomingOut());
    } 

    IEnumerator ZoomingOut()
    {
        while( !attending.isTreating() &&  Camera.main.orthographicSize < camStartZoom - 0.2f)
        {
            yield return  Camera.main.orthographicSize < camStartZoom;
            gameCam.orthographicSize = Mathf.Lerp(gameCam.orthographicSize, camStartZoom, Time.deltaTime * 2); 
            gameCam.transform.position = Vector3.Lerp(gameCam.transform.position, camStartPos, Time.deltaTime * 2);
            levelSystem.GetButton(3).gameObject.SetActive(true);
        }
    }  

}



