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
                print("�÷��̾� ����");
            if (hp > 100)
                hp = 100;
            Debug.Log("�÷��̾� HP :" + hp.ToString());
            image.fillAmount = hp / 100;
            if (damage > 0)
                StartCoroutine(ShowVignette());
        }
        else
            print("��������");
        
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
