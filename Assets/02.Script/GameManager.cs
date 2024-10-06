using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   [SerializeField]private Transform startSpot;
   [SerializeField]private Transform deadSpot;
   [SerializeField]private List<GameObject> plants;
   [SerializeField]private TextMeshProUGUI scoretext;
   [SerializeField]private Image nextPlantImage;
   [SerializeField]private GameObject nextPlant;
   [SerializeField]private Button replayGameBtn;
   [SerializeField]private GameObject gameoverUi;
   
   private static GameManager instance;

   private bool isGameEnd;

   public bool IsGameEnd
   {
       get
       {
           return isGameEnd;
       }
       set
       {
           isGameEnd = value;
       }
   }

   public static GameManager Instance
   {
       get
       {
           if (!instance)
           {
               instance = FindObjectOfType(typeof(GameManager)) as GameManager;

               if (instance == null)
               {
                   Debug.Log("no Singleton obj");
               }

           }
               return instance;
       }
   }
   
   
   private int randomNum;

   private void Awake()
   {
       gameoverUi.SetActive(false);
       
       isGameEnd = false;
       
       replayGameBtn.onClick.AddListener(() =>
       {
           //현재의 씬 다시 불러오기
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       });
       
       if (instance == null)
       {
           instance = this;
           
       }else if (instance != this)
       {
           Debug.Log("여긴 안오지 ?");
           Destroy(gameObject);
       }
       
       //DontDestroyOnLoad(gameObject);
       
       
       randomNum = 0;

       scoretext.text = "0";

       Instantiate(plants[randomNum],startSpot.position, Quaternion.identity).transform.parent = startSpot;
       
       nextPlantImage.sprite = plants[randomNum].GetComponent<SpriteRenderer>().sprite;

       GetNextPlant();
   }

   public void GetNextPlant()
   {
       randomNum = Random.Range(0,plants.Count);
       
       GameObject nextPlant = plants[randomNum];
       
       nextPlantImage.sprite = nextPlant.GetComponent<SpriteRenderer>().sprite;

       this.nextPlant = nextPlant;
   }

   //새 행성 추가
   public void SetNewPlant()
   {
       if (!isGameEnd)
       {
           StartCoroutine(WaitForIt());
       }
   }

   public void AddScore(int score)
   {
       scoretext.text= (int.Parse(scoretext.text)+score).ToString();
   }

   public void SetGameOver()
   {
       isGameEnd = true;
       gameoverUi.SetActive(true);
   }
   
   IEnumerator WaitForIt()
   {
       startSpot.GetChild(0).parent = deadSpot;
       
       yield return new WaitForSeconds(2.0f);
       
       GameObject newPlant = Instantiate(nextPlant, startSpot.position, Quaternion.identity);
       
       newPlant.transform.parent = startSpot;

       newPlant.GetComponent<Rigidbody2D>().isKinematic = true;
       
       GetNextPlant();   
       
   }
   
}
