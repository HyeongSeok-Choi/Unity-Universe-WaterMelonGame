using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{

    //완전히 안착했다는 기준을 무엇으로 잡지 ?
    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.transform.IsChildOf(transform))
        {
            //StartWarningLight();
            float deadzoneRadius = GetComponent<CircleCollider2D>().bounds.center.x -
                                   GetComponent<Collider2D>().bounds.min.x;
          // Debug.Log(deadzoneRadius+"데드존 반지름");

           float plantzoneRadius = other.GetComponent<CircleCollider2D>().bounds.center.x -
                                   other.GetComponent<Collider2D>().bounds.min.x;
        //  Debug.Log(plantzoneRadius+"플랜트 반지름");
           
           float distace = Vector2.Distance(other.transform.position, transform.position);
         //  Debug.Log(distace+"거리");

         if (distace > deadzoneRadius - plantzoneRadius && other.GetComponent<Plant>().isArrive)
         {
             other.GetComponent<Plant>().OutTime += Time.deltaTime;
    
             if (other.GetComponent<Plant>().OutTime > 3f)
             {
                 GameManager.Instance.SetGameOver();
             }
         }
         else
            {
                other.GetComponent<Plant>().StayTime += Time.deltaTime;
                if (other.GetComponent<Plant>().StayTime > 3f)
                {
                    other.GetComponent<Plant>().isArrive = true;
                }  
            }
        }
        
    }

    // private void StartWarningLight()
    // {
    //     Color color = GetComponent<SpriteRenderer>().color;
    //         
    //     float f = 0f;
    //     
    //     while (f < 255f)
    //     {
    //         f += 1f;
    //         color.r = f;
    //         GetComponent<SpriteRenderer>().color = color;
    //     }
    //
    //     while (f > 0f)
    //     {
    //         f -= 1f;
    //         color.r = f;
    //         GetComponent<SpriteRenderer>().color = color;
    //     }
    //
    // }
    
}
