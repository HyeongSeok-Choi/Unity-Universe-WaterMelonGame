using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   [SerializeField]private Transform startZone;
   [SerializeField]public  Transform deadZone;
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
   private int plantMaxIndex;

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
       plantMaxIndex = plants.Count - 2;
       replayGameBtn.onClick.AddListener(() =>
       {
           //현재의 씬 다시 불러오기
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       });
       if (instance == null)
       {
           instance = this;
           
       }
       randomNum = 0;
       score = 0;
       scoreText.text = string.Format("{0}", score);;
       GameObject startPlant = Instantiate(plants[randomNum],startZone.position, Quaternion.identity);
       startPlant.transform.parent = startZone;
       startPlant.GetComponent<Rigidbody2D>().isKinematic = true;
       nextPlantImage.sprite = plants[randomNum].GetComponent<SpriteRenderer>().sprite;
       GetNextPlant();
   }

   public void GetNextPlant()
   {
       if (smallPlantShootCount > smallPlantShootLimit)
       {
           plantMaxIndex = plants.Count;
       }
       randomNum = Random.Range(0, plantMaxIndex);
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
       //plant의 collision 이벤트 함수와의 시점 동기화를 위함
       //Destroy 적용이 되기 전 목록이 뜸
       //아직까지 완벽한 확인은 힘들지만 예상하기로는 이벤트 함수와 히어라이키 창의 동기화가 되는데 조금의 시간이 필요해서가 아닐지 ?
       yield return new WaitForSeconds(0.8f);
       Transform[] deadZonePlant = deadZone.transform.GetComponentsInChildren<Transform>();
       for (int i = 1; i < deadZonePlant.Length; i++)
       {
           yield return new WaitForSeconds(0.8f);
           AddScore(deadZonePlant[i].GetComponent<Plant>().PlantScore);
           PlayParticle(deadZonePlant[i].position);
           Destroy(deadZonePlant[i].gameObject);
       }
   }
   
   private IEnumerator WaitForItNewPlant()
   {
       if (!isGameEnd)
       {
           isSpawnPlant = false;
           smallPlantShootCount += 1;
           startZone.GetChild(0).parent = deadZone;
           yield return new WaitForSeconds(1.0f);
           GameObject newPlant = Instantiate(nextPlant, startZone.position, Quaternion.identity);
           isSpawnPlant = true;
           newPlant.transform.parent = startZone;
           newPlant.GetComponent<CircleCollider2D>().enabled = false;
           newPlant.GetComponent<Rigidbody2D>().isKinematic = true;
           GetNextPlant();
       }
   }
}
