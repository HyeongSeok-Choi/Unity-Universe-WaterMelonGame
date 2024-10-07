using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   [SerializeField]private Transform startSpot;
   [SerializeField]private Transform deadSpot;
   [SerializeField]private List<GameObject> plants;
   [SerializeField]private TextMeshProUGUI scoreText;
   [SerializeField]private Image nextPlantImage;
   [SerializeField]private GameObject nextPlant;
   [SerializeField]private Button replayGameBtn;
   [SerializeField]private GameObject gameoverUi;
   [SerializeField]private int smallPlantShootCount;
   [SerializeField]private int smallPlantShootLimit;
   
   private static GameManager instance;
   private bool isGameEnd;
   private bool isSpawnPlant;
   private int randomNum;

   public bool IsGameEnd
   {
       get { return isGameEnd; }
       set { isGameEnd = value; }
   }

   public bool IsSpawnPlant
   {
       get { return isSpawnPlant; }
       set { isSpawnPlant = value; }
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
   private void Awake()
   {
       smallPlantShootCount = 0;
       smallPlantShootLimit = 8;
       gameoverUi.SetActive(false);
       isGameEnd = false;
       isSpawnPlant = true;
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
           Destroy(gameObject);
       }
       
       //DontDestroyOnLoad(gameObject);
       randomNum = 0;
       scoreText.text = "0";
       GameObject go = Instantiate(plants[randomNum],startSpot.position, Quaternion.identity);
       go.transform.parent = startSpot;
       go.GetComponent<CircleCollider2D>().enabled = false;
       nextPlantImage.sprite = plants[randomNum].GetComponent<SpriteRenderer>().sprite;
       GetNextPlant();
   }

   public void GetNextPlant()
   {
       int maxIndex = plants.Count;
       
       if (smallPlantShootCount < smallPlantShootLimit)
       {
           maxIndex = plants.Count - 2;
       }
       
       randomNum = Random.Range(0, maxIndex);
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
       scoreText.text= string.Format("{0}", score);
   }

   public void SetGameOver()
   {
       isGameEnd = true;
       gameoverUi.SetActive(true);
   }
   
   IEnumerator WaitForIt()
   {
       if (!isGameEnd)
       {
           isSpawnPlant = false;
           smallPlantShootCount += 1;
           startSpot.GetChild(0).parent = deadSpot;
           yield return new WaitForSeconds(2.0f);
           GameObject newPlant = Instantiate(nextPlant, startSpot.position, Quaternion.identity);
           isSpawnPlant = true;
           newPlant.transform.parent = startSpot;
           newPlant.GetComponent<CircleCollider2D>().enabled = false;
           newPlant.GetComponent<Rigidbody2D>().isKinematic = true;
           GetNextPlant();
       }

   }
   
}
