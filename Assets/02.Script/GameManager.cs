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
   [SerializeField]public Transform deadSpot;
   [SerializeField]private List<GameObject> plants;
   [SerializeField]private TextMeshProUGUI scoreText;
   [SerializeField]private Image nextPlantImage;
   [SerializeField]private GameObject nextPlant;
   [SerializeField]private Button replayGameBtn;
   [SerializeField]private GameObject gameoverUi;
   [SerializeField]private int smallPlantShootCount;
   [SerializeField]private int smallPlantShootLimit;
   [SerializeField]private GameObject destroyedParticle;
   [SerializeField]private TextMeshProUGUI gameEndMessage;
   
   private static GameManager instance;
   private bool isGameEnd;
   private bool isGameWin;
   private bool isSpawnPlant;
   private int randomNum;
   private int score;

   public bool IsGameEnd
   {
       get { return isGameEnd; }
       set { isGameEnd = value; }
   }

   public bool IsGameWin
   {
       get { return isGameWin; }
       set { isGameWin = value; }
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
       isGameWin = false;
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
       score = 0;
       scoreText.text = string.Format("{0}", score);;
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
           StartCoroutine(WaitForItNewPlant());
       }
   }

   public void AddScore(int score)
   {
       this.score += score;
       scoreText.text= string.Format("{0}", this.score);
   }

   public void SetGameEnd(bool isGameWin)
   {
       isGameEnd = true;
       if (isGameWin)
       {
           StartCoroutine(DestroyAllPlants());
           gameEndMessage.text = "You Win !!";
       }
       else
       {
           gameEndMessage.text = "Game Over";   
       }
       gameoverUi.SetActive(true);
   }
   
   public void PlayParticle(Vector3 particlePosition)
   {
       GameObject particle =Instantiate(destroyedParticle, particlePosition, Quaternion.identity);
       particle.GetComponent<ParticleSystem>().Play();
   }

   private IEnumerator DestroyAllPlants()
   {
       yield return new WaitForSeconds(0.8f);
       Transform[] deadzonePlant = deadSpot.transform.GetComponentsInChildren<Transform>();
       for (int i = 1; i < deadzonePlant.Length-1; i++)
       {
           yield return new WaitForSeconds(0.8f);
           PlayParticle(deadzonePlant[i].position);
           Destroy(deadzonePlant[i].gameObject);
       }
   }
   
   private IEnumerator WaitForItNewPlant()
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
