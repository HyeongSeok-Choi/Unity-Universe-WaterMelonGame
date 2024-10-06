using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Plant : MonoBehaviour
{
    
    [SerializeField]private List<GameObject> plants;
    private Transform deadZone;
    private int plantIndex;
    private bool isFirst;
    public bool isArrive;
    private int plantScore;
    private float stayTime;
    private float outTime;

    public float StayTime
    {
        get
        {
            return stayTime;
        }
        set
        {
            stayTime = value;
        }
    }

    public float OutTime
    {
        get
        {
            return outTime;
        }
        set
        {
            outTime = value;
        }
    }
    void Start()
    {
        outTime = 0f;
        stayTime = 0f;
        isArrive = false;
        isFirst = true;
        plantScore = 0;
        //현재 플랜트 인덱스 계산
        for (int i = 0; i < plants.Count; i++)
        {
            if (gameObject.CompareTag(plants[i].tag))
            {
                plantIndex = i;

                if (plantIndex == 0)
                {
                    plantScore += 10;
                }
                else
                {
                    plantScore += plantIndex * 20;
                }
            }
        }    
    }
    
    //동시 충돌 이벤트 발생문제
    //gameobject를 삭제해도, 컴포넌트를 지워도 해결되지 않음
    //그러나 필드에 접근 해서 bool값을 통해 2번째엔 접근이 안되게 해서 해결은함
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (gameObject.CompareTag(other.gameObject.tag)&&isFirst)
        {
                Destroy(other.gameObject);
                Destroy(gameObject);
                GameManager.Instance.AddScore(plantScore*2);
                other.gameObject.GetComponent<Plant>().isFirst = false;
                GameObject mergePlant=  Instantiate(plants[plantIndex + 1], transform.position, quaternion.identity);
                if (mergePlant.CompareTag("Pluto"))
                {
                    Debug.Log("게임 이김");
                    GameManager.Instance.SetGameOver();
                }

                mergePlant.transform.parent = GameObject.Find("DeadZone").GetComponent<Transform>();
        }
    }
}
