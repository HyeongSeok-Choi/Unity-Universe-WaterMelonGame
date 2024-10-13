using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    [SerializeField]private LineRenderer dragLineRenderer;
    [SerializeField]private LineRenderer predictDragLineRenderer;
    [SerializeField]private float zoomInSpeed = 5f;
    [SerializeField]private float zoomOutSpeed = 5f;
    [SerializeField]private float limitDistance = 20f;
    [SerializeField]private float shootingPower = 4;
    [SerializeField]private float magneticPower = 30f;
    [SerializeField]private float predictTime = 0.007f;
    [SerializeField]private int positionCount = 60;

    private Camera mainCamera;
    private float nomalCameraSize;
    private Vector3 mouseClickPosition;
    private Vector3 realDragOffset;
    private Rigidbody2D shootingPlanetRigidBody;
    
    private void Awake()
    {
        mainCamera = Camera.main;
        nomalCameraSize = mainCamera.orthographicSize;
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsGameEnd && GameManager.Instance.IsSpawnPlanet)
        {
            mouseClickPosition = GetMousePos();
            dragLineRenderer.SetPosition(0, GameManager.Instance.StartZone.position);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsGameEnd&& GameManager.Instance.IsSpawnPlanet)
        {
            dragLineRenderer.enabled = true;
            realDragOffset = GetMousePos() - mouseClickPosition;
            Vector3 lineDragPosition = GameManager.Instance.StartZone.position + realDragOffset;
            if (realDragOffset.magnitude >= limitDistance)
            {         
                lineDragPosition = (realDragOffset.normalized) * limitDistance + GameManager.Instance.StartZone.position;
            }
            dragLineRenderer.SetPosition(1, lineDragPosition);
            //선분의 길이에 무조건 비례하게 !!!!
            float zoomDistance = realDragOffset.magnitude;
            if (zoomDistance > limitDistance)
            {  
                zoomDistance = limitDistance;
            }else if (zoomDistance <= 0)
            {
                zoomDistance = 0f;
            }
            mainCamera.orthographicSize =
                Mathf.Lerp(mainCamera.orthographicSize,nomalCameraSize + zoomDistance,Time.deltaTime * zoomOutSpeed);
            CalculationPredict();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsGameEnd&& GameManager.Instance.IsSpawnPlanet)
        {
            StartCoroutine(ResizeCamera());
            dragLineRenderer.enabled = false;
            predictDragLineRenderer.enabled = false;
            if (GameManager.Instance.StartZone.childCount >= 1)
            {
                shootingPlanetRigidBody = GameManager.Instance.StartZone.GetChild(0).GetComponent<Rigidbody2D>();
                shootingPlanetRigidBody.isKinematic = false;
                shootingPlanetRigidBody.GetComponent<CircleCollider2D>().isTrigger = false;
                realDragOffset = GetMousePos() - mouseClickPosition;
                float dragPower = realDragOffset.magnitude;
                if (dragPower > limitDistance)
                {
                    dragPower = limitDistance;
                }
                Vector3 dir = realDragOffset.normalized;
                shootingPlanetRigidBody.velocity = -dir * shootingPower * dragPower;
                GameManager.Instance.SetNewPlanet();
            }
        }
    }
    private Vector3 GetMousePos(){
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; 
        return mousePos;
    }
    private IEnumerator ResizeCamera()
    {
        while (true)
        {
            if (mainCamera.orthographicSize <= nomalCameraSize)
            {
                break;
            }
            mainCamera.orthographicSize =
                Mathf.Lerp(mainCamera.orthographicSize,mainCamera.orthographicSize - zoomInSpeed,Time.deltaTime * zoomInSpeed);
            yield return null;
        }
    }

    private void CalculationPredict()
    {
        predictDragLineRenderer.positionCount = positionCount;
        predictDragLineRenderer.SetPosition(0, GameManager.Instance.StartZone.position);
        predictDragLineRenderer.enabled = true;
        
        float dragPower = realDragOffset.magnitude;
        if (dragPower > limitDistance)
        {
            dragPower = limitDistance;
        }
        Vector3 dir = realDragOffset.normalized;
        Vector3 pVelocity = -dir * shootingPower * dragPower;
        Vector3 pStartPosition = GameManager.Instance.StartZone.position;
        Vector3 deadZoneDir= GameManager.Instance.DeadZone.position.normalized;
        for(int predictIndex = 1; predictIndex < predictDragLineRenderer.positionCount ; predictIndex++)
        {
            float time = predictIndex * predictTime;
            Vector3 pos = pStartPosition + pVelocity * time + 0.5f * deadZoneDir * magneticPower  * time * time;
            pos.z = -1f;
          
            predictDragLineRenderer.SetPosition(predictIndex, pos);
          
        }
    }
}
