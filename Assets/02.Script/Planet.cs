using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField]private List<GameObject> planets;
    
    private GameObject deadZone;
    private int planetIndex;
    private int planetScore;
    private float planetRadius;
    private float stayTime;
    private float outTime;
    private bool isFirst;

    public float StayTime
    { 
        get { return stayTime; }
        set { stayTime = value; }
    }

    public float OutTime
    {
        get { return outTime; }
        set { outTime = value; }
    }
    
    public float PlanetRadius
    { 
        get { return planetRadius; }
    }
    
    public int PlanetScore
    { 
        get { return planetScore; }
    }
    
    private void Awake()
    {   
        outTime = 0f;
        stayTime = 0f;
        isFirst = true;
        planetRadius = GetComponent<CircleCollider2D>().bounds.center.x -
                      GetComponent<CircleCollider2D>().bounds.min.x;
        
        //현재 플랜트 인덱스 계산
        for (int i = 0; i < planets.Count; i++)
        {
            if (gameObject.CompareTag(planets[i].tag))
            {
                planetIndex = i;
                if (planetIndex == 0)
                {
                    planetScore = 10;
                }
                else
                {
                    planetScore = planetIndex * 20;
                }
            }
        }    
    }
    
    //동시 충돌 이벤트 발생문제
    //gameobject를 삭제해도, 컴포넌트를 지워도 해결되지 않음
    //그러나 필드에 접근 해서 bool값을 통해 2번째엔 접근이 안되게 해서 해결은함
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject.CompareTag(other.gameObject.tag)&&isFirst&&!GameManager.Instance.IsGameEnd)
        {
                Destroy(gameObject);
                GameManager.Instance.PlayParticle(gameObject.transform.position);
                Destroy(other.gameObject);
                GameManager.Instance.PlayParticle(other.gameObject.transform.position);
                GameManager.Instance.AddScore(planetScore*2);
                other.gameObject.GetComponent<Planet>().isFirst = false;
                GameObject mergePlanet=  Instantiate(planets[planetIndex + 1], transform.position, Quaternion.identity);
                mergePlanet.transform.parent = GameManager.Instance.DeadZone.transform;
                if (mergePlanet.CompareTag(planets[planets.Count-1].tag))
                {
                    GameManager.Instance.IsGameWin = true;
                    GameManager.Instance.SetGameEnd(GameManager.Instance.IsGameWin);
                }
        }
    }
}
