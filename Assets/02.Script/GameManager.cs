using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   [SerializeField]private Transform startSpot;
   [SerializeField]private Transform deadSpot;
   [SerializeField]private List<GameObject> plants;
   [SerializeField]private TextMeshProUGUI scoretext;

   private int randomNum;

   private void Awake()
   {
       randomNum = 0;

       scoretext.text = "0";

       Instantiate(plants[randomNum],startSpot.position, Quaternion.identity).transform.parent = startSpot;
   }
   
   
   //새 행성 추가
   public void SetNewPlant()
   {
       StartCoroutine(WaitForIt());
   }

   public void AddScore(int score)
   {
       scoretext.text= (int.Parse(scoretext.text)+score).ToString();
   }
   
   IEnumerator WaitForIt()
   {
       startSpot.GetChild(0).parent = deadSpot;
       
       yield return new WaitForSeconds(2.0f);

       randomNum = Random.Range(0,plants.Count);

       GameObject newPlant = Instantiate(plants[randomNum], startSpot.position, Quaternion.identity);
       
       newPlant.transform.parent = startSpot;

       newPlant.GetComponent<Rigidbody2D>().isKinematic = true;
       
   }
   
}
