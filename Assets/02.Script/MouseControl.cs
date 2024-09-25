using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    public float deg;
    public float turrentSpeed;
    public GameObject turrent;

    void Start()
    {
        deg = 0f;
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            deg = deg + Time.deltaTime * turrentSpeed;
            turrent.transform.eulerAngles = new Vector3(0, 0, deg);
        }else if (Input.GetKey(KeyCode.DownArrow))
        {
            deg = deg - Time.deltaTime * turrentSpeed;
            turrent.transform.eulerAngles = new Vector3(0, 0, deg);
        }
        
    }
}
