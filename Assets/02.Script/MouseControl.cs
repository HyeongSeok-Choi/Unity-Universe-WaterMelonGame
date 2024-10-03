using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour
{
    private Camera mainCamera;
    
    private Vector3 startPosition;
    
    private Vector3 dragOffset;

    private float z;
    
    
    //그렇다면 Power을 점점 세게 바꾸는거야 드래그가 늘어날수록 ?
    [SerializeField] private float power = 30f;
    [SerializeField] private float rotationPower = 100f;
    [SerializeField] private Transform deadZone;
    private GameManager gameManager;
    [SerializeField]private Transform startSpot;

    private Rigidbody2D rb;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    private void OnMouseUp()
    {
        if (startSpot.childCount >= 1)
        {
            rb = startSpot.GetChild(0).GetComponent<Rigidbody2D>();
            rb.isKinematic = false;
            dragOffset = startPosition - GetMousePos();
            rb.AddForce(dragOffset * power);

            gameManager.SetNewPlant();
        }
    }
    
    private void OnMouseDown()
    {
        startPosition = GetMousePos();
    }

    Vector3 GetMousePos(){
        var mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        // 마우스의 위치값 가져오기
        mousePos.z = 0; 
        return mousePos; // 마우스 위치값 반환 
    }
}
