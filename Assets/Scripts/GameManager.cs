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
    public Transform makerTr;
    public Boss_Elemental elemental;
    public GameObject bossTime;

    public Animator leftDoorAnim;
    public Animator rightDoorAnim;
    public bool isGargoyleDead;
    public bool isStart = false;
    public GameObject doorStar;
    public GameObject stage0Canvas;
    public GameObject stage1Canvas;
    // public GameObject teleportInteractor;

    //public RawImage image;
    [SerializeField]
    private string firstScene = "stage00";
    [SerializeField]
    private string secondScene = "stage01";
    [SerializeField] private Animator fadeAnim;
    private Vector3 stage0Position = new Vector3(193f, 7.2f, 71.5f);
    private Vector3 stage1Position = new Vector3(-32.5f, 6f, 22.5f);
    private Vector3 bossZone0 = new Vector3(-20f, 1f, 235f);
    private Vector3 returnStage0 = new Vector3(107f, 2f, 244f);
    private Vector3 centerHall = new Vector3(2.5f, 6f, -17.5f);
    private Vector3 deadZone = new Vector3(-10f, -1f, 100f);

    private Vector3 bossZone1 = new Vector3(2.5f, 1f, 33.5f);

    [SerializeField] private GameObject leftHand;
    [SerializeField] private GameObject rightHand;
    [SerializeField] private GameObject box;
    [SerializeField] private GameObject plane;
    [SerializeField] private XRRayInteractor leftInteractor;
    [SerializeField] private XRRayInteractor rightInteractor;
    //[SerializeField] private GameObject canvasStart;
    [SerializeField] private GameObject tutorials;
    [SerializeField] private GameObject leftmodel;
    [SerializeField] private GameObject rightmodel;
    [SerializeField] private Image image;
    [SerializeField] private PlayerDamage playerDamage;
    [SerializeField] private ControllManager controllManager;



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
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        controllManager.IsPosible = false;
        Debug.Log("마법 불가능");
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator ToTutorial()
    {
        FadeOut();
        yield return new WaitForSeconds(1f);
        HideController();
        yield return new WaitForSeconds(1f);
        box.SetActive(false);
        FadeIn();
    }

    private IEnumerator ToStage0()
    {
        if (!firstScene.Equals(""))
        {
            FadeOut();
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(firstScene);
            playerTr.position = stage0Position;
            leftHand.transform.position = stage0Position;
            rightHand.transform.position = stage0Position;
            HideController();
            playerTr.rotation = Quaternion.Euler(0, 270f, 0);

            yield return new WaitForSeconds(1.5f);
            FadeIn();

            isStart = true;
            controllManager.IsPosible = true;
            Debug.Log("마법 가능");
            stage0Canvas.SetActive(true);
        }
    }

    private IEnumerator ToStage1()
    {
        if (!secondScene.Equals(""))
        {
            FadeOut();
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(secondScene);
            playerTr.position = stage1Position;
            leftHand.transform.position = stage1Position;
            rightHand.transform.position = stage1Position;
            HideController();
            playerTr.rotation = Quaternion.Euler(0, 180f, 0);
            makerTr.localScale = new Vector3(9f, 9f, 9f);
            yield return new WaitForSeconds(1.5f);
            FadeIn();

            isStart = true;
            controllManager.IsPosible = true;
            Debug.Log("마법 가능");
            stage1Canvas.SetActive(true);
        }
    }
    private IEnumerator FadeScreen0()
    {
        if (!firstScene.Equals(""))
        {
            FadeOut();
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(firstScene);
            playerTr.position = stage0Position;
            leftHand.transform.position = stage0Position;
            rightHand.transform.position = stage0Position;
            playerTr.rotation = Quaternion.Euler(0, 270f, 0);

            yield return new WaitForSeconds(1.5f);
            FadeIn();
            stage0Canvas.SetActive(true);
        }
    }
    private IEnumerator FadeScreen1()
    {
        if (!secondScene.Equals(""))
        {
            FadeOut();
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(secondScene);
            playerTr.position = stage1Position;
            leftHand.transform.position = stage1Position;
            rightHand.transform.position = stage1Position;
            playerTr.rotation = Quaternion.Euler(0, 180f, 0);
            makerTr.localScale = new Vector3(9f, 9f, 9f);

            yield return new WaitForSeconds(1.5f);
            FadeIn();
            stage1Canvas.SetActive(true);
        }
    }
    private IEnumerator FadeScreenBoss0()
    {
        FadeOut();
        yield return new WaitForSeconds(1f);

        playerTr.position = bossZone0;
        leftHand.transform.position = bossZone0;
        rightHand.transform.position = bossZone0;
        playerTr.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(1.5f);
        FadeIn();
    }

    private IEnumerator ReturnToStage0()
    {
        bossTime.SetActive(true);
        yield return new WaitForSeconds(3);
        FadeOut();
        yield return new WaitForSeconds(1f);

        playerTr.position = returnStage0;
        leftHand.transform.position = returnStage0;
        rightHand.transform.position = returnStage0;
        playerTr.rotation = Quaternion.Euler(0, 0, 0);
        bossTime.SetActive(false);
        yield return new WaitForSeconds(1f);
        FadeIn();
    }

    private IEnumerator MoveToCenterHall()
    {
        FadeOut();
        yield return new WaitForSeconds(1f);
        playerTr.position = centerHall;
        leftHand.transform.position = centerHall;
        rightHand.transform.position = centerHall;
        playerTr.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(1f);
        FadeIn();
    }
    //죽었을때
    private IEnumerator ToDeadZone()
    {
        image.color = Color.red;
        FadeOut();
        controllManager.IsPosible = false;
        Debug.Log("마법 불가능");
        yield return new WaitForSeconds(1.5f);

        playerTr.position = deadZone;
        leftHand.transform.position = deadZone;
        rightHand.transform.position = deadZone;
        playerTr.rotation = Quaternion.Euler(0, 0, 0);
        yield return new WaitForSeconds(2f);
        isStart = false;
        ShowController();
        FadeIn();
        image.color = Color.black;
        playerDamage.getDamage(-100);
    }
    private IEnumerator GameQuit()
    {
        FadeOut();
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
    private void FadeOut()
    {
        fadeAnim.SetBool("fadein", false);
    }
    private void FadeIn()
    {
        fadeAnim.SetBool("fadein", true);
    }
    //튜토리얼 끝나고 포탈로 스테이지0로 감
    public void Stage0Load()
    {
        StartCoroutine(FadeScreen0());
    }
    //정상적으로 스테이지1로 감
    public void Stage1Load()
    {
        StartCoroutine(FadeScreen1());
    }
    //스테이지0 보스존으로
    public void Boss0Load()
    {
        StartCoroutine(FadeScreenBoss0());
    }
    //스테이지0 보스 죽이고 다시 맵으로 감
    public void Boss0Kill()
    {
        StartCoroutine(ReturnToStage0());
    }
    //가고일 방으로 감
    public void CenterHallStage1()
    {
        StartCoroutine(MoveToCenterHall());
    }

    public void PlayerDead()
    {
        StartCoroutine(ToDeadZone());
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
    //튜토리얼을 한다
    public void tutorialStart()
    {
        StartCoroutine(ToTutorial());
        plane.SetActive(true);
        tutorials.SetActive(true);
        isStart = true;
        controllManager.IsPosible = true;
        Debug.Log("마법 가능");
    }
    //튜토리얼 안하고 스테이지0로 갈 때, 죽고 다시 시작
    public void PassTutorial()
    {
        StartCoroutine(ToStage0());
    }
    //스테이지1 죽고 다시 시작
    public void RestartStage1()
    {
        StartCoroutine(ToStage1());

        controllManager.icePosible = false;
        controllManager.bookSpell[2].SetActive(false);
    }

    public void ShowController()
    {
        leftHand.SetActive(false);
        rightHand.SetActive(false);
        leftmodel.SetActive(true);
        rightmodel.SetActive(true);
        leftInteractor.enabled = true;
        rightInteractor.enabled = true;
    }

    public void HideController()
    {
        leftInteractor.enabled = false;
        rightInteractor.enabled = false;
        leftmodel.SetActive(false);
        rightmodel.SetActive(false);
        leftHand.SetActive(true);
        rightHand.SetActive(true);
    }
    public void GameClear()
    {
        StartCoroutine(GameQuit());
    }
    //임시 스테이지1 보스로 감
    public void Stage1Boss()
    {
        StartCoroutine(FadeBoss1());
    }
    //임시 스테이지1 보스
    private IEnumerator FadeBoss1()
    {
        if (!secondScene.Equals(""))
        {
            FadeOut();
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(secondScene);
            playerTr.position = bossZone1;
            leftHand.transform.position = bossZone1;
            rightHand.transform.position = bossZone1;
            playerTr.rotation = Quaternion.Euler(0, 0, 0);
            makerTr.localScale = new Vector3(9f, 9f, 9f);

            yield return new WaitForSeconds(1.5f);
            FadeIn();
        }
    }
}
