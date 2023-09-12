using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour, IDamage
{
    public int hp = 100;
    [SerializeField] Image image;
    [SerializeField] GameObject vignette;
    public bool isSuper;
    public void getDamage(int damage)
    {
        if (!isSuper)
        {
            hp -= damage;
            if (hp <= 0)
                GameManager.Instance.PlayerDead();
            else if (hp >= 100)
                hp = 100;

            //Debug.Log("플레이어 HP :" + hp.ToString());
            
            image.fillAmount = hp / 100f;
            if (damage > 0 && hp > 0)
                StartCoroutine(ShowVignette());
        }
        else
            print("무적상태");

    }

    public void StrongMagic(int damage)
    {
        hp -= damage;
        image.fillAmount = hp / 100f;
    }

    // Start is called before the first frame update
    void Start()
    {
        isSuper = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator ShowVignette()
    {
        vignette.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        vignette.SetActive(false);
    }
}
