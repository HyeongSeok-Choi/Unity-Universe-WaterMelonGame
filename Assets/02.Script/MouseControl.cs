using System.Collections;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    [SerializeField] private LineRenderer dragLineRenderer;
    [SerializeField] private float limitDistance;
    [SerializeField] private float power;
    [SerializeField] private float rotationPower;
    [SerializeField] private Transform deadZone;
    [SerializeField] private Transform startSpot;
    [SerializeField] private float zoomInSpeed;
    [SerializeField] private float zoomOutSpeed;
    
    private Transform[] linePoints;
    private Camera mainCamera;
    private Vector3 mouseClickPosition;
    private Vector3 dragOffset;
    private float nomalCameraSize;
    private float RotateZ;
    private Rigidbody2D shootingPlantRigidBody;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        nomalCameraSize = mainCamera.orthographicSize;
    }

    private void Start()
    {
        shootingPlantRigidBody = startSpot.GetChild(0).GetComponent<Rigidbody2D>();
        shootingPlantRigidBody.isKinematic = true;
    }

    private void Update()
    {   
        RotateZ+= Time.deltaTime* rotationPower;
        if (startSpot.childCount >= 1)
        {
            startSpot.GetChild(0).transform.eulerAngles= new Vector3(0f, 0f, RotateZ);
        }
    }
    
    private void OnMouseDown()
    {
        if (!GameManager.Instance.IsGameEnd && GameManager.Instance.IsSpawnPlant)
        {
            mouseClickPosition = GetMousePos();
            dragLineRenderer.SetPosition(0, startSpot.position);
        }
    }

    private void OnMouseDrag()
    {
        if (!GameManager.Instance.IsGameEnd&& GameManager.Instance.IsSpawnPlant)
        {
                dragLineRenderer.enabled = true;
                dragOffset = mouseClickPosition - GetMousePos();
                Vector3 dragPosition = startSpot.position - dragOffset;
                if (dragOffset.magnitude >= limitDistance)
                {         
                        dragPosition = (-dragOffset.normalized) * limitDistance + startSpot.position;
                }
                dragLineRenderer.SetPosition(1, dragPosition);
                float zoomDistance = Vector2.Distance(mouseClickPosition, GetMousePos()) * Time.deltaTime;
                
                if (nomalCameraSize + limitDistance <= mainCamera.orthographicSize)
                {
                    zoomDistance = 0f;
                }
                mainCamera.orthographicSize += zoomDistance * zoomInSpeed;
        }
    }
    
    private void OnMouseUp()
    {
        if (!GameManager.Instance.IsGameEnd&& GameManager.Instance.IsSpawnPlant)
        {
            StartCoroutine(ResizeCamera());
            dragLineRenderer.enabled = false;
            if (startSpot.childCount >= 1)
            {
                startSpot.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                shootingPlantRigidBody = startSpot.GetChild(0).GetComponent<Rigidbody2D>();
                shootingPlantRigidBody.isKinematic = false;
                dragOffset = mouseClickPosition - GetMousePos();
                float dragPower = Vector2.Distance(mouseClickPosition, GetMousePos());
                if (dragPower > limitDistance)
                {
                    dragPower = limitDistance;
                }
                Vector3 dir = dragOffset.normalized;
                shootingPlantRigidBody.AddForce(dir * power * dragPower);
                GameManager.Instance.SetNewPlant();
            }
        }
    }
    
    Vector3 GetMousePos(){
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; 
        return mousePos;
    }
    
    private IEnumerator ResizeCamera()
    {
        bool isSizing = true;
        while (isSizing)
        {
            if (mainCamera.orthographicSize <= nomalCameraSize)
            {
                isSizing = false;
            }
            mainCamera.orthographicSize -= Vector2.Distance(mouseClickPosition, GetMousePos()) * Time.deltaTime*zoomOutSpeed;
            yield return null;
        }
    }
}
