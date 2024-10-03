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
            // {
            // Debug.Log(other.name+"자식");
            // Debug.Log(Vector2.Distance(other.transform.position,transform.position)+"디스턴스");
            //}
            if (this.GetComponent<Collider2D>().bounds.Contains(other.bounds.min) &&
                this.GetComponent<Collider2D>().bounds.Contains(other.bounds.max))
            {
                Debug.Log(other.name + "영역 안 포함");
            }
            else
            {
                Debug.Log(other.name + "영역 밖");
            }
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.IsChildOf(transform))
        {
           // Debug.Log(other.name+"집 나간 자식");
        }
    }
    
}
