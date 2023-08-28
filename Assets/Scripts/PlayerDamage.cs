using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour, IDamage
{
    public int hp = 100;
    [SerializeField] Image image;
    [SerializeField] GameObject vignette;

    public void getDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
            print("플레이어 죽음");
        if (hp > 100)
            hp = 100;
        //Debug.Log("플레이어 HP :" + hp.ToString());
        image.fillAmount = hp/100;
        if (damage > 0)
            StartCoroutine(ShowVignette());
    }

    // Start is called before the first frame update
    void Start()
    {

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
