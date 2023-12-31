using DigitalRuby.ThunderAndLightning;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class ControllManager : MonoBehaviour
{
    public GameObject spellBook;
    public Transform playerTr;
    public InputActionProperty openBook;
    public InputActionProperty indexChange;
    public LightningSpellScript spell;
    float leftGripValue;
    public bool IsPosible;

    private WaitForSeconds lightningWait = new WaitForSeconds(0.6f);

    public Transform rightControllerTr;
    public Transform leftControllerTr;
    public PlayerDamage playerDamage;
    public GameObject sheild;
    public GameObject[] selectedSpell;
    public GameObject[] magicPrefabs;
    public GameObject[] bookSpell;
    public int index;
    public bool lightningPosible;
    public bool icePosible;
    public Collider lightningCollider;

    [SerializeField] GameObject leftHand;
    [SerializeField] GameObject teleportInteractor;

    //public TMP_Text text;
    private void Awake()
    {


    }
    void Start()
    {
        teleportInteractor.SetActive(false);
        index = 0;
        spellBook.SetActive(false);
        bookSpell[0].SetActive(true);
        SelectSpell(0);
        GestureRecognition gr = new GestureRecognition();
        lightningCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        OpenBook();
    }
    void OpenBook()
    {
        if (GameManager.Instance.isStart)
        {
            leftGripValue = openBook.action.ReadValue<float>();
            if (leftGripValue > 0.9f)
            {
                spellBook.SetActive(true);
                leftHand.SetActive(false);
                teleportInteractor.SetActive(false);
            }
            else
            {
                spellBook.SetActive(false);
                leftHand.SetActive(true);
                teleportInteractor.SetActive(true);
            }
        }
    }

    //왼쪽 제스쳐 인식
    public void IndexChange(string rec)
    {
        if (rec == "0")
        {
            index = 0;
            //ShowSpell();
            SelectSpell(0);
        }
        else if (rec == "1" && lightningPosible)
        {
            index = 1;
            //ShowSpell();
            SelectSpell(1);
        }
        else if (rec == "2" & icePosible)
        {
            index = 2;
            //ShowSpell();
            SelectSpell(2);
        }



        /*if (isOpen)
        {
            float xbutton = indexChange.action.ReadValue<float>();
            if (xbutton > 0.9f && !isButton)
            {
                StartCoroutine(XButton());
                index++;
                if (index >= 3)
                    index = 0;

                SelectSpell(index);
                // Debug.Log("버튼X");
            }
        }*/
    }
    /*IEnumerator XButton()
    {
        isButton = true;
        yield return buttonWait;
        isButton = false;
    }*/

    //오른쪽 제스쳐 인식
    public void MotionMagic(string rec)
    {
        if (rec == "O" && IsPosible)
        {
            IsPosible = false;
            Vector3 shieldTransform = Camera.main.transform.position + Camera.main.transform.forward * 0.4f - Camera.main.transform.up * 0.2f;
            GameObject shelid = Instantiate(sheild, shieldTransform, Camera.main.transform.rotation);
            Destroy(shelid, 3f);
            StartCoroutine(MagicIsPosible(new WaitForSeconds(3.3f)));
        }
        else if (rec == "Slash" && IsPosible)
        {
            if (index == 0) //파이어볼
            {
                IsPosible = false;
                Vector3 newPosition = rightControllerTr.position + Camera.main.transform.forward * 0.1f;
                GameObject magic = Instantiate(magicPrefabs[index], newPosition, Quaternion.identity);
                Destroy(magic, 1.2f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(1.5f)));


            }
            else if (index == 1 && lightningPosible) //라이트닝
            {
                IsPosible = false;
                Vector3 playerDirection = Camera.main.transform.forward;
                //playerDirection.y = 0f;
                spell.Direction = playerDirection;
                spell.SpellStart.transform.position = rightControllerTr.position + Camera.main.transform.forward * 0.1f;
                StartCoroutine(LightningSpell());
                StartCoroutine(MagicIsPosible(new WaitForSeconds(1.5f)));

            }
            else if (index == 2 && icePosible) // 얼음화살
            {
                IsPosible = false;
                Vector3 newPosition = rightControllerTr.position + Camera.main.transform.forward * 0.1f;
                //Quaternion rotationNoY = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                GameObject magic = Instantiate(magicPrefabs[index - 1], newPosition, Camera.main.transform.rotation);
                Destroy(magic, 1.5f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(1.5f)));

            }
        }
        else if (rec == "X" && IsPosible)
        {
            
            if (index == 0) //메테오
            {
                if (playerDamage.hp > 20)
                {
                    IsPosible = false;
                    playerDamage.StrongMagic(20);
                    Vector3 spawnPosition = Camera.main.transform.position + playerTr.forward * 5f + Vector3.up * 5f;
                    GameObject magic = Instantiate(magicPrefabs[index + 2], spawnPosition, Quaternion.identity);
                    Destroy(magic, 4f);
                    StartCoroutine(MagicIsPosible(new WaitForSeconds(4f)));
                }

            }
            else if (index == 1 && lightningPosible) //천둥 다발
            {
                if (playerDamage.hp > 20)
                {
                    IsPosible = false;
                    playerDamage.StrongMagic(20);
                    Vector3 spawnPosition = Camera.main.transform.position + playerTr.forward * 5f;
                    //Debug.Log("스폰 위치 y값" + spawnPosition.y);
                    //Debug.Log("카메라높이" + Camera.main.transform.position.y);
                    GameObject magic = Instantiate(magicPrefabs[index + 2], spawnPosition, Quaternion.identity);
                    Destroy(magic, 4f);
                    StartCoroutine(MagicIsPosible(new WaitForSeconds(4f)));
                }
            }
            else if (index == 2 && icePosible) //블리자드
            {
                if (playerDamage.hp > 20)
                {
                    IsPosible = false;
                    playerDamage.StrongMagic(20);

                    //Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 5f + Vector3.up * 5f;
                    //GameObject magic = Instantiate(magicPrefabs[index +1], spawnPosition, Quaternion.Euler(90f, 0, 0));            
                    //Destroy(magic, 3f);
                    //StartCoroutine(MagicIsPosible(new WaitForSeconds(3.3f)));
                    Vector3 spawnPosition = Camera.main.transform.position + playerTr.forward * 5f + Vector3.up * 5f;
                    //spawnPosition.y = 1f;
                    GameObject magic = Instantiate(magicPrefabs[index + 2], spawnPosition, Quaternion.Euler(90f, 0, 0));
                    Destroy(magic, 3.6f);
                    StartCoroutine(MagicIsPosible(new WaitForSeconds(4f)));
                }
            }
        }
        else if(rec =="Star"&& GameManager.Instance.isGargoyleDead)
        {
            GameManager.Instance.DoorOpen();
        }
        else
            Debug.Log("안맞음");
    }
    IEnumerator LightningSpell()
    {
        lightningCollider.enabled = true;
        spell.CastSpell();
        yield return lightningWait;
        spell.StopSpell();
        lightningCollider.enabled = false;
    }
    //마법발사 쿨타임
    IEnumerator MagicIsPosible(WaitForSeconds wait)
    {
        yield return wait;
        IsPosible = true;
    }

    private void SelectSpell(int index)
    {
        StartCoroutine(ShowMagicSpell(index));
    }
    //3D 제스쳐 인식
    public void OnGestureCompleted(GestureCompletionData data)
    {
        if (data.gestureID < 0)
        {
            string errorMessage = GestureRecognition.getErrorMessage(data.gestureID);
            return;
        }


        if (data.similarity >= 0.4f)
        {
            Debug.Log(data.similarity + "+" + data.gestureName);
            //text.text = data.similarity + "+" + data.gestureName;
        }

    }
    /*private void ShowSpell()
    {
        Vector3 newPosition = Camera.main.transform.position + Camera.main.transform.forward * 0.5f + Vector3.down * 0.3f;
        GameObject spell = Instantiate(selectedSpell, newPosition, Camera.main.transform.rotation);
    }*/

    IEnumerator ShowMagicSpell(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            selectedSpell[i].SetActive(i == index);
        }
        yield return new WaitForSeconds(1.5f);
        selectedSpell[index].SetActive(false);
    }
}
