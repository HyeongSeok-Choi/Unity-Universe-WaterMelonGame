using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseControl : MonoBehaviour, IBeginDragHandler, IDragHandler,IEndDragHandler
{
    [SerializeField]private LineRenderer dragLineRenderer;
    [SerializeField]private float zoomInSpeed = 20f;
    [SerializeField]private float zoomOutSpeed = 0.4f;
    [SerializeField]private float limitDistance = 20f;
    [SerializeField]private float shootingPower = 200f;
    
    private Camera mainCamera;
    private Vector3 mouseClickPosition;
    private Vector3 realDragOffset;
    private float nomalCameraSize;
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
            realDragOffset = mouseClickPosition - GetMousePos();
            Vector3 lineDragPosition = GameManager.Instance.StartZone.position - realDragOffset;
            if (realDragOffset.magnitude >= limitDistance)
            {         
                lineDragPosition = (-realDragOffset.normalized) * limitDistance + GameManager.Instance.StartZone.position;
            }
            dragLineRenderer.SetPosition(1, lineDragPosition);
            float zoomDistance = realDragOffset.magnitude * Time.deltaTime;
                
            if (nomalCameraSize + limitDistance <= mainCamera.orthographicSize)
            {
                zoomDistance = 0f;
            }
            Mathf.Lerp(mainCamera.orthographicSize,mainCamera.orthographicSize += zoomDistance,Time.deltaTime*zoomOutSpeed);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!GameManager.Instance.IsGameEnd&& GameManager.Instance.IsSpawnPlanet)
        {
            StartCoroutine(ResizeCamera());
            dragLineRenderer.enabled = false;
            if (GameManager.Instance.StartZone.childCount >= 1)
            {
                GameManager.Instance.StartZone.GetChild(0).GetComponent<CircleCollider2D>().enabled = true;
                shootingPlanetRigidBody = GameManager.Instance.StartZone.GetChild(0).GetComponent<Rigidbody2D>();
                shootingPlanetRigidBody.isKinematic = false;
                realDragOffset = mouseClickPosition - GetMousePos();
                float dragPower = Vector3.Distance(mouseClickPosition, GetMousePos());
                if (dragPower > limitDistance)
                {
                    dragPower = limitDistance;
                }
                Vector3 dir = realDragOffset.normalized;
                shootingPlanetRigidBody.AddForce(dir * shootingPower * dragPower);
                GameManager.Instance.SetNewPlanet();
            }
        }
    }
    private Vector3 GetMousePos(){
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; 
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
            Mathf.Lerp(mainCamera.orthographicSize,mainCamera.orthographicSize -= Time.deltaTime*zoomInSpeed,Time.deltaTime*zoomOutSpeed);
            yield return null;
        }
    }
}
