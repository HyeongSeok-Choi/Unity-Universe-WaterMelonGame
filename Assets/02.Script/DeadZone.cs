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
            float deadzoneRadius = GetComponent<CircleCollider2D>().bounds.center.x -
                                 GetComponent<Collider2D>().bounds.min.x;
            float plantzoneRadius = other.GetComponent<CircleCollider2D>().bounds.center.x -
                                   other.GetComponent<Collider2D>().bounds.min.x;
            float distance = Vector2.Distance(other.transform.position, transform.position);

         if (distance > deadzoneRadius - plantzoneRadius && other.GetComponent<Plant>())
         {
             other.GetComponent<Plant>().StayTime = 0f;
             other.GetComponent<Plant>().OutTime += Time.deltaTime;
             
             if (other.GetComponent<Plant>().OutTime > 3f)
             {
                 GameManager.Instance.IsGameWin = false;
                 GameManager.Instance.SetGameEnd(GameManager.Instance.IsGameWin);
             }
         }
         else
            {
                other.GetComponent<Plant>().StayTime += Time.deltaTime;
                
                if (other.GetComponent<Plant>().StayTime > 3f)
                {
                    other.GetComponent<Plant>().OutTime = 0f;
                }  
            }
        }
        
    }
}
