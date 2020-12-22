using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuredBringer : MonoBehaviour
{
    [SerializeField] float speed = 3;
    [SerializeField] Soldier soldier;
    bool reached, dropped;
    float startPos;
    int xDrop;
   
    // Start is called before the first frame update
    void Start()
    { 
       dropped = false;
       reached = false;
       startPos = transform.position.x;
       xDrop = Random.Range(-9,10);
    }

    // Update is called once per frame
    void Update()
    {
        if(!reached)
        {
            if(transform.position.x != xDrop)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(xDrop,transform.position.y), speed * Time.deltaTime);
            }
            else
            {
                StartCoroutine(DropInjured());
                reached = true;
            }
        }
          
          
         
        if(reached && dropped)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(-startPos,transform.position.y), speed * Time.deltaTime);
            

            if(transform.position.x <= -startPos)
            {
                Destroy(gameObject);
            }
        }
        
        
    }

    IEnumerator DropInjured()
    {
        yield return new WaitUntil(()=> transform.position.x == xDrop);
        yield return new WaitForSeconds(1);
        if(!dropped)
        {
            Instantiate(soldier,transform.position,Quaternion.identity);
            dropped = true;
        }
          
    }
}
