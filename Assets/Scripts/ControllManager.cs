using DigitalRuby.ThunderAndLightning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ControllManager : MonoBehaviour
{
    public GameObject spellBook;
    public InputActionProperty openBook;
    public InputActionProperty castMagic;
    public InputActionProperty indexChange;
    public LightningSpellScript spell;
    float leftGripValue;
    float rightGripValue;
    bool isOpen;
    bool isButton;
    bool IsPosible;
    private WaitForSeconds buttonWait = new WaitForSeconds(0.5f);
    private WaitForSeconds lightningWait = new WaitForSeconds(0.6f);

    public Transform cameraTr;
    public Transform rightControllerTr;
    public GameObject sheild;
    //public ParticleSystem[] magicEffects;
    public GameObject[] magicPrefabs;
    public GameObject[] magicSpell;
    int index;


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
    }

    // Update is called once per frame
    void Update()
    {
        OpenBook();
        //CastMagic();
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
    /*void CastMagic()
    {
        rightGripValue = castMagic.action.ReadValue<float>();
        if (rightGripValue > 0.9f)
        {
            magicEffects[index].Play();
            isFire = true;
        }
        else if (rightGripValue <= 0.9f && rightGripValue > 0)
        {
            magicEffects[index].Stop();
            //StartCoroutine(FireMagic());
            isFire = false;
        }
        else
            magicEffects[index].Stop();
    }

    IEnumerator FireMagic()
    {
        while (isFire)
        {
            Vector3 newPosition = rightControllerTr.position + cameraTr.forward * 0.2f;
            // 마법 생성 (날아가게 할 것임)
            GameObject magic = Instantiate(magicPrefabs[index], newPosition, Camera.main.transform.rotation);

            yield return new WaitForSeconds(0.5f);
        }
    }*/
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
                Vector3 newPosition = rightControllerTr.position + cameraTr.forward * 0.1f;
                // 마법 생성 
                GameObject magic = Instantiate(magicPrefabs[index], newPosition, Camera.main.transform.rotation);
                Destroy(magic, 1.2f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(1.4f)));
            }
            else if (index == 1)
            {
                Vector3 playerDirection = Camera.main.transform.forward;
                playerDirection.y = 0f;
                spell.Direction = playerDirection;
                spell.SpellStart.transform.position = rightControllerTr.position + cameraTr.forward * 0.1f;
                StartCoroutine(LightningSpell());
                StartCoroutine(MagicIsPosible(new WaitForSeconds(0.9f)));
            }
            else if (index == 2)
            {
                //Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 5f + Vector3.up * 10f;
                //GameObject magic = Instantiate(magicPrefabs[index - 1], spawnPosition, Quaternion.Euler(90f, 0, 0));
                // Destroy(magic, 4f);
                Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * 5f;
                spawnPosition.y = 1f;   
                GameObject magic = Instantiate(magicPrefabs[index - 1], spawnPosition, Quaternion.identity);
                Destroy(magic, 2f);
                StartCoroutine(MagicIsPosible(new WaitForSeconds(2.3f)));
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
}
