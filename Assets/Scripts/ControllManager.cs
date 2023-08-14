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
    float rightGripValue;
    bool isOpen;
    bool isButton;
    bool IsPosible;
    private WaitForSeconds buttonWait = new WaitForSeconds(0.5f);
    private WaitForSeconds lightningWait = new WaitForSeconds(0.6f);


    public Transform rightControllerTr;
    public GameObject sheild;
    //public ParticleSystem[] magicEffects;
    public GameObject[] magicPrefabs;
    public GameObject[] magicSpell;
    int index;

    //public TMP_Text text;
    private void Awake()
    {


    }
    void Start()
    {
        index = 0;
        spellBook.SetActive(false);
        isOpen = false;
        isButton = false;
        IsPosible = true;
        SelectSpell(0);
        GestureRecognition gr = new GestureRecognition();
    }

    // Update is called once per frame
    void Update()
    {
        OpenBook();
        IndexChange();

    }
    void OpenBook()
    {
        leftGripValue = openBook.action.ReadValue<float>();
        if (leftGripValue > 0.9f)
        {
            spellBook.SetActive(true);
            isOpen = true;
        }
        else
        {
            spellBook.SetActive(false);
            isOpen = false;
        }
    }

    void IndexChange()
    {
        if (isOpen)
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
        }

    }
    IEnumerator XButton()
    {
        isButton = true;
        yield return buttonWait;
        isButton = false;

    }
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
            IsPosible = false;
            if (index == 0)
            {
                Vector3 newPosition = rightControllerTr.position + Camera.main.transform.forward * 0.1f;
                GameObject magic = Instantiate(magicPrefabs[index], newPosition, Camera.main.transform.rotation);
                Destroy(magic, 1.2f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(1.4f)));
            }
            else if (index == 1)
            {
                Vector3 playerDirection = Camera.main.transform.forward;
                playerDirection.y = 0f;
                spell.Direction = playerDirection;
                spell.SpellStart.transform.position = rightControllerTr.position + Camera.main.transform.forward * 0.1f;
                StartCoroutine(LightningSpell());
                StartCoroutine(MagicIsPosible(new WaitForSeconds(0.9f)));
            }
            else if (index == 2)
            {
                
                Vector3 newPosition = rightControllerTr.position + Camera.main.transform.forward * 0.1f;
                Quaternion rotationNoY = Quaternion.Euler(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
                GameObject magic = Instantiate(magicPrefabs[index - 1], newPosition, rotationNoY);
                Destroy(magic, 1.2f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(1.4f)));

            }
        }
        else if (rec == "X" && IsPosible)
        {
            IsPosible = false;
            if (index == 0)
            {
                Vector3 spawnPosition = Camera.main.transform.position + playerTr.forward * 5f + Vector3.up * 5f;
                GameObject magic = Instantiate(magicPrefabs[index + 3], spawnPosition, Quaternion.identity);
                Destroy(magic, 3f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(3.3f)));
            }
            else if (index == 1)
            {
                Vector3 spawnPosition = Camera.main.transform.position + playerTr.forward * 5f;
                //Debug.Log("스폰 위치 y값" + spawnPosition.y);
                //Debug.Log("카메라높이" + Camera.main.transform.position.y);
                GameObject magic = Instantiate(magicPrefabs[index + 1], spawnPosition, Quaternion.identity);
                Destroy(magic, 4f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(4f)));
            }
            else if (index == 2)
            {
                //Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 5f + Vector3.up * 5f;
                //GameObject magic = Instantiate(magicPrefabs[index +1], spawnPosition, Quaternion.Euler(90f, 0, 0));            
                //Destroy(magic, 3f);
                //StartCoroutine(MagicIsPosible(new WaitForSeconds(3.3f)));
                Vector3 spawnPosition = Camera.main.transform.position + playerTr.forward * 1f;
                //spawnPosition.y = 1f;
                GameObject magic = Instantiate(magicPrefabs[index + 2], spawnPosition, Quaternion.identity);
                Destroy(magic, 3f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(3.3f)));
            }
        }
        else
            Debug.Log("안맞음");
    }
    IEnumerator LightningSpell()
    {
        spell.CastSpell();
        yield return lightningWait;
        spell.StopSpell();
    }
    //마법발사 쿨타임
    IEnumerator MagicIsPosible(WaitForSeconds wait)
    {
        yield return wait;
        IsPosible = true;
    }

    void SelectSpell(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            magicSpell[i].SetActive(i == index);
        }
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
}
