using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private float deadzoneRadius;
    private void Awake()
    {
        deadzoneRadius = GetComponent<CircleCollider2D>().bounds.center.x -
                         GetComponent<Collider2D>().bounds.min.x;
    }

    //완전히 안착했다는 기준을 무엇으로 잡지 ?
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform) && other.GetComponent<Plant>())
        {
            float plantzoneRadius = other.GetComponent<Plant>().PlantzoneRadius;
            float distance = Vector2.Distance(other.transform.position, transform.position);
            
         if (distance > deadzoneRadius* 1.2f - plantzoneRadius)
         {  Debug.Log("뇽뇽");
             other.GetComponent<Plant>().StayTime = 0f;
             other.GetComponent<Plant>().OutTime += Time.deltaTime;
             if (other.GetComponent<Plant>().OutTime > 3f)
             {
                 GameManager.Instance.IsGameWin = false;
                 GameManager.Instance.SetGameEnd(GameManager.Instance.IsGameWin);
             }
         }
         else
         {  Debug.Log("냥냥");
                other.GetComponent<Plant>().StayTime += Time.deltaTime;
                if (other.GetComponent<Plant>().StayTime > 3f)
                {
                    other.GetComponent<Plant>().OutTime = 0f;
                }  
            }
        }
    }
    
}
