using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamage : MonoBehaviour, IDamage
{
    int hp = 100;
    [SerializeField] Image image;
    [SerializeField] GameObject vignette;

    public void getDamage(int damage)
    {
        hp -= damage;  
        image.fillAmount = hp;
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
