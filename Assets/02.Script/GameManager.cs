using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
   [SerializeField]private List<GameObject> planets;
   [SerializeField]private Transform startZone;
   [SerializeField]private Transform deadZone;
   [SerializeField]private TextMeshProUGUI scoreText;
   [SerializeField]private TextMeshProUGUI gameEndMessage;
   [SerializeField]private GameObject gameoverUi;
   [SerializeField]private GameObject nextPlanet;
   [SerializeField]private Image nextPlanetImage;
   [SerializeField]private Button replayGameBtn;
   [SerializeField]private int smallPlanetShootCount;
   [SerializeField]private int smallPlanetShootLimit = 8;
   [SerializeField]private float respawnTime = 1.0f;
   [SerializeField]private float planetDestoryTime = 0.8f;
   [SerializeField]private float rotationPower = 100f;
   [SerializeField]private GameObject destroyedParticle;

   private static GameManager instance;
   
   private float RotateZ;
   private bool isGameEnd = false;
   private bool isGameWin = false;
   private bool isSpawnPlanet = true;
   private int randomNum = 0;
   private int score = 0;
   private int planetMaxIndex;

   public Transform DeadZone
   {
       get { return deadZone; }
   }
   
   public Transform StartZone
   {
       get { return startZone; }
   }

   public bool IsGameEnd
   {
       get { return isGameEnd; }
   }

   public bool IsGameWin
   {
       get { return isGameWin; }
       set { isGameWin = value; }
   }

   public bool IsSpawnPlanet
   {
       get { return isSpawnPlanet; }
   }

   public static GameManager Instance
   {
       get
       { 
           if (instance == null)
           {
               Debug.Log("instance is null");
           }
           return instance;
       }
   }
   private void Awake()
   {
       gameoverUi.SetActive(false);
       planetMaxIndex = planets.Count - 2;
       replayGameBtn.onClick.AddListener(() =>
       {
           //현재의 씬 다시 불러오기
           SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
       });
       if (instance == null)
       {
           instance = this;
       }
       scoreText.text = string.Format("{0}", score);
       GameObject startPlanet = Instantiate(planets[randomNum],startZone.position, Quaternion.identity);
       startPlanet.transform.parent = startZone;
       startPlanet.GetComponent<Rigidbody2D>().isKinematic = true;
       startPlanet.GetComponent<CircleCollider2D>().isTrigger = true;
       GetNextPlanet();
   }
   
   private void FixedUpdate()
   {   
       if (startZone.childCount >= 1)
       {   RotateZ+= Time.fixedDeltaTime* rotationPower;
           startZone.GetChild(0).rotation = Quaternion.Euler(new Vector3(0f, 0f, RotateZ));
       }
   }

   public void GetNextPlanet()
   {
       if (smallPlanetShootCount > smallPlanetShootLimit)
       {
           planetMaxIndex = planets.Count;
       }
       randomNum = Random.Range(0, planetMaxIndex);
       GameObject nextPlanet = planets[randomNum];
       nextPlanetImage.sprite = nextPlanet.GetComponent<SpriteRenderer>().sprite;
       this.nextPlanet = nextPlanet;
   }

   //새 행성 추가
   public void SetNewPlanet()
   {
       if (!isGameEnd)
       {
           StartCoroutine(WaitForItNewPlanet());
       }
   }

   public void AddScore(int score)
   {
       this.score += score;
       scoreText.text= string.Format("{0}", this.score);
   }

   public void SetGameEnd()
   {
       isGameEnd = true;
       if (isGameWin)
       {
           StartCoroutine(DestroyAllPlanets());
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

   private IEnumerator DestroyAllPlanets()
   {
       //planet의 collision 이벤트 함수와의 시점 동기화를 위함
       //Destroy 적용이 되기 전 목록이 뜸
       //아직까지 완벽한 확인은 힘들지만 예상하기로는 이벤트 함수와 하이라이키 창의 동기화가 되는데 조금의 시간이 필요해서가 아닐지 ?
       yield return new WaitForSeconds(planetDestoryTime);
       Transform[] deadZonePlanet = deadZone.transform.GetComponentsInChildren<Transform>();
       for (int childIndex = 1; childIndex < deadZonePlanet.Length; childIndex++)
       {
           yield return new WaitForSeconds(planetDestoryTime);
           AddScore(deadZonePlanet[childIndex].GetComponent<Planet>().PlanetScore);
           PlayParticle(deadZonePlanet[childIndex].position);
           Destroy(deadZonePlanet[childIndex].gameObject);
       }
   }
   
   private IEnumerator WaitForItNewPlanet()
   {
           isSpawnPlanet = false;
           smallPlanetShootCount += 1;
           if (startZone.childCount >= 1)
           {
               startZone.GetChild(0).parent = deadZone;
           }
           yield return new WaitForSeconds(respawnTime);
           isSpawnPlanet = true;
           GameObject newPlanet = Instantiate(nextPlanet, startZone.position, Quaternion.identity);
           newPlanet.transform.parent = startZone;
           newPlanet.GetComponent<Rigidbody2D>().isKinematic = true;
           newPlanet.GetComponent<CircleCollider2D>().isTrigger = true;
           GetNextPlanet();
       }
}
