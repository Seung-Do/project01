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
        FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator FadeScreen0()
    {      
        if (!firstScene.Equals(""))
        {
            FadeOut();
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(firstScene);
            playerTr.position = stage0Position;
            handsTr.position = stage0Position + Vector3.up;
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
            handsTr.position = stage1Position + Vector3.up;
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
        handsTr.position = bossZone0 + Vector3.up;
        playerTr.rotation = Quaternion.Euler(0, 0, 0);

        yield return new WaitForSeconds(2f);
        FadeIn();
    }

    private IEnumerator ReturnToStage0()
    {
        FadeOut();
        yield return new WaitForSeconds(1f);

        playerTr.position = returnStage0;
        handsTr.position = returnStage0 + Vector3.up;
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
}
