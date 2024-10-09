using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    [SerializeField] private LineRenderer dragLineRenderer;
    [SerializeField] private Transform startZone;
    [SerializeField] private Transform deadZone;
    [SerializeField] private float zoomInSpeed;
    [SerializeField] private float zoomOutSpeed;
    [SerializeField] private float limitDistance;
    [SerializeField] private float shootingPower;
    [SerializeField] private float rotationPower;
    
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

    private void Update()
    {   
        RotateZ+= Time.deltaTime* rotationPower;
        if (startZone.childCount >= 1)
        {
            startZone.GetChild(0).transform.eulerAngles= new Vector3(0f, 0f, RotateZ);
        }
    }
    
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsGameEnd && GameManager.Instance.IsSpawnPlant)
        {
            mouseClickPosition = GetMousePos();
            dragLineRenderer.SetPosition(0, startZone.position);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsGameEnd&& GameManager.Instance.IsSpawnPlant)
        {
            dragLineRenderer.enabled = true;
            dragOffset = mouseClickPosition - GetMousePos();
            Vector3 dragPosition = startZone.position - dragOffset;
            if (dragOffset.magnitude >= limitDistance)
            {         
                dragPosition = (-dragOffset.normalized) * limitDistance + startZone.position;
            }
            dragLineRenderer.SetPosition(1, dragPosition);
            float zoomDistance = dragOffset.magnitude * Time.deltaTime;
                
            if (nomalCameraSize + limitDistance <= mainCamera.orthographicSize)
            {
                zoomDistance = 0f;
            }
            Mathf.Lerp(mainCamera.orthographicSize,mainCamera.orthographicSize += zoomDistance,Time.deltaTime/zoomOutSpeed);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsGameEnd&& GameManager.Instance.IsSpawnPlant)
        {
            StartCoroutine(ResizeCamera());
            dragLineRenderer.enabled = false;
            if (startZone.childCount >= 1)
            {
                startZone.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                shootingPlantRigidBody = startZone.GetChild(0).GetComponent<Rigidbody2D>();
                shootingPlantRigidBody.isKinematic = false;
                dragOffset = mouseClickPosition - GetMousePos();
                float dragPower = Vector2.Distance(mouseClickPosition, GetMousePos());
                if (dragPower > limitDistance)
                {
                    dragPower = limitDistance;
                }
                Vector3 dir = dragOffset.normalized;
                shootingPlantRigidBody.AddForce(dir * shootingPower * dragPower);
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
            Mathf.Lerp(mainCamera.orthographicSize,mainCamera.orthographicSize -= Time.deltaTime*zoomInSpeed,Time.deltaTime/zoomOutSpeed);
            yield return null;
        }
    }
}
