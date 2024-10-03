using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Plant : MonoBehaviour
{
    
    [SerializeField]private List<GameObject> plants;
    private GameManager gameManager;
    private Transform deadZone;
    private int plantIndex;
    private bool isFirst;
    private int plantScore;
    void Start()
    {
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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
                gameManager.AddScore(plantScore*2);
                other.gameObject.GetComponent<Plant>().isFirst = false;
                GameObject mergePlant=  Instantiate(plants[plantIndex + 1], transform.position, quaternion.identity);
                mergePlant.transform.parent = GameObject.Find("DeadZone").GetComponent<Transform>();
        }
    }
}
