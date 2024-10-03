using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCamera : MonoBehaviour
{
    // private Camera mainCam;
    // private Vector3 mousePosition;
    // void Start()
    // {
    //     mainCam = GetComponent<Camera>();
    // }
    //
    // // Update is called once per frame
    // void Update()
    // {
    //     //raycast연습
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
    //         
    //         Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
    //         
    //         RaycastHit2D hit = Physics2D.Raycast(mousePosition, transform.forward, 15f);
    //         
    //         if (hit)
    //         {
    //            Debug.Log(hit.transform.name);
    //         }
    //     
    //     }
    // }
}
