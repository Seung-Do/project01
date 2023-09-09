using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    public PoolManager[] poolManager;
    public Transform playerTr;
    public Transform handsTr;
    public Boss_Elemental elemental;
    public GameObject bossTime;

    public Animator leftDoorAnim;
    public Animator rightDoorAnim;
    public bool isGargoyleDead;
    public bool isStart = false;
    public GameObject doorStar;
    public GameObject teleportInteractor;

    //public RawImage image;
    [SerializeField]
    private string firstScene = "stage00";
    [SerializeField]
    private string secondScene = "stage01";
    [SerializeField] private Animator fadeAnim;
    private Vector3 stage0Position = new Vector3(193f, 7.2f, 71.5f);
    private Vector3 stage1Position = new Vector3(-32.5f, 15.1f, 22.5f);
    private Vector3 bossZone0 = new Vector3(-20f, 20f, 235f);
    private Vector3 returnStage0 = new Vector3(107f, 20f, 244f);
    private Vector3 CenterHall = new Vector3(2.5f, 20f, -17.5f);

    [SerializeField] private GameObject box;
    [SerializeField] private XRRayInteractor leftInteractor;
    [SerializeField] private XRRayInteractor rightInteractor;
    [SerializeField] private GameObject canvasStart;
    
 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
  
    }
    void Start()
    {
        teleportInteractor.SetActive(false);
        FadeIn();    
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStart)
        {
            playerTr.position = new Vector3(0, 0.11f, -10f);
            playerTr.rotation = Quaternion.identity;
        }
    }
    private IEnumerator JustFade()
    {
        FadeOut();
        yield return new WaitForSeconds(1.5f);
        FadeIn();
        box.SetActive(false);
    }
    private IEnumerator FadeScreen0()
    {      
        if (!firstScene.Equals(""))
        {
            FadeOut();
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(firstScene);
            playerTr.position = stage0Position;
            handsTr.position = stage0Position;
            playerTr.rotation = Quaternion.Euler(0, 270f, 0);

            yield return new WaitForSeconds(1.5f);
            FadeIn();
        }       
    }
    private IEnumerator FadeScreen1()
    {
        if (!firstScene.Equals(""))
        {
            FadeOut();
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(secondScene);
            playerTr.position = stage1Position;
            handsTr.position = stage1Position;
            playerTr.rotation = Quaternion.Euler(0, 180f, 0);

            yield return new WaitForSeconds(1.5f);
            FadeIn();
        }
    }
    private IEnumerator FadeScreenBoss0()
    {
        FadeOut();
        yield return new WaitForSeconds(1f);

        playerTr.position = bossZone0;
        handsTr.position = bossZone0;
        playerTr.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(2f);
        FadeIn();
    }

    private IEnumerator ReturnToStage0()
    {
        bossTime.SetActive(true);
        yield return new WaitForSeconds(3);
        FadeOut();
        yield return new WaitForSeconds(1f);

        playerTr.position = returnStage0;
        handsTr.position = returnStage0;
        playerTr.rotation = Quaternion.Euler(0, 0, 0);
        bossTime.SetActive(false);
        yield return new WaitForSeconds(2f);
        FadeIn();
    }

    private IEnumerator MoveToCenterHall()
    {
        FadeOut();
        yield return new WaitForSeconds(1f);
        playerTr.position = CenterHall;
        handsTr.position = CenterHall;
        playerTr.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(2f);
        FadeIn();
    }
    private void FadeOut()
    {
        fadeAnim.SetBool("fadein", false);
    }
    private void FadeIn()
    {
        fadeAnim.SetBool("fadein", true);
    }

    public void Stage0Load()
    {
        StartCoroutine(FadeScreen0());
    }
    public void Stage1Load()
    {
        StartCoroutine(FadeScreen1());
    }
    public void Boss0Load()
    {
        StartCoroutine(FadeScreenBoss0());
    }
    public void Boss0Kill()
    {
        StartCoroutine(ReturnToStage0());
    }
    public void CenterHallStage1()
    {
        StartCoroutine(MoveToCenterHall());
    }

    public void DoorOpen()
    {
        leftDoorAnim.SetBool("open", true);
        rightDoorAnim.SetBool("open", true);
        doorStar.SetActive(false);
    }
    public void DoorClose()
    {
        leftDoorAnim.SetBool("open", false);
        rightDoorAnim.SetBool("open", false);
    }

    public void tutorialStart()
    {
        StartCoroutine(JustFade());
        leftInteractor.enabled = false;
        rightInteractor.enabled = false;
        canvasStart.SetActive(false);
        isStart = true;
    }

    public void PassTutorial()
    {
        leftInteractor.enabled = false;
        rightInteractor.enabled = false;
        Stage0Load();
        isStart = true;
    }
}
