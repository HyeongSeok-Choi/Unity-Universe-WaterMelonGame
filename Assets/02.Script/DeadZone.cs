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
        if (other.transform.IsChildOf(transform) && other.GetComponent<Planet>())
        {
            float planetzoneRadius = other.GetComponent<Planet>().PlanetRadius;
            float distance = Vector3.Distance(other.transform.position, transform.position);
            
         if (distance > deadzoneRadius - planetzoneRadius)
         {  Debug.Log("밖");
             other.GetComponent<Planet>().StayTime = 0f;
             other.GetComponent<Planet>().OutTime += Time.deltaTime;
             if (other.GetComponent<Planet>().OutTime > 3f)
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
                other.GetComponent<Planet>().StayTime += Time.deltaTime;
                if (other.GetComponent<Planet>().StayTime > 3f)
                {
                    other.GetComponent<Planet>().OutTime = 0f;
                }  
            }
        }
    }
}
