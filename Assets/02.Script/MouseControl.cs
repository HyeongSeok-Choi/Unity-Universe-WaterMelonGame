using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour
{

    [SerializeField] private LineRenderer line;
    
    private Transform[] linePoints;
    
    private Camera mainCamera;
    
    private Vector3 startPosition;
    
    private Vector3 dragOffset;
    
    private float z;
    
    [SerializeField] private float power = 30f;
    [SerializeField] private float rotationPower = 100f;
    [SerializeField] private Transform deadZone;
    [SerializeField] private Transform startSpot;
    
    private Rigidbody2D rb;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        rb = startSpot.GetChild(0).GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    private void Update()
    {   
        z+= Time.deltaTime* rotationPower;
        if (startSpot.childCount >= 1)
        {
            startSpot.GetChild(0).transform.eulerAngles= new Vector3(0f, 0f, z);
        }
    }
    
    private void OnMouseDown()
    {
        startPosition = GetMousePos();
        line.SetPosition(0,startSpot.position); 
    }

    private void OnMouseDrag()
    {
        line.enabled = true;
        dragOffset = startPosition - GetMousePos();
        line.SetPosition(1,startSpot.position - dragOffset); 
    }
    
    private void OnMouseUp()
    {
        line.enabled = false;
        if (startSpot.childCount >= 1)
        {
            rb = startSpot.GetChild(0).GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            dragOffset = startPosition - GetMousePos();
            rb.AddForce(dragOffset * power);
            
            GameManager.Instance.SetNewPlant();
        }
    }

    Vector3 GetMousePos(){
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // 마우스의 위치값 가져오기
        mousePos.z = 0; 
        return mousePos; // 마우스 위치값 반환 
    }
}
