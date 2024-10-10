using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private float deadzoneRadius;
    private void Awake()
    {
        deadzoneRadius = GetComponent<CircleCollider2D>().bounds.center.x -
                         GetComponent<CircleCollider2D>().bounds.min.x;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform) && other.GetComponent<Plant>())
        {
            float plantzoneRadius = other.GetComponent<Plant>().PlantRadius;
            float distance = Vector3.Distance(other.transform.position, transform.position);
            
         if (distance > deadzoneRadius - plantzoneRadius)
         {  Debug.Log("밖");
             other.GetComponent<Plant>().StayTime = 0f;
             other.GetComponent<Plant>().OutTime += Time.deltaTime;
             if (other.GetComponent<Plant>().OutTime > 3f)
             {
                 if (!GameManager.Instance.IsGameWin)
                 {
                     GameManager.Instance.IsGameWin = false;
                     GameManager.Instance.SetGameEnd(GameManager.Instance.IsGameWin);
                 }
             }
         }
         else
         {  Debug.Log("안");
                other.GetComponent<Plant>().StayTime += Time.deltaTime;
                if (other.GetComponent<Plant>().StayTime > 3f)
                {
                    other.GetComponent<Plant>().OutTime = 0f;
                }  
            }
        }
    }
}
