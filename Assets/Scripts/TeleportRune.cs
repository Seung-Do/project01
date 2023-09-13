using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRune : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject effect;
    [SerializeField] GameObject nextEffect;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            target.SetActive(false);
            effect.SetActive(false);
            nextEffect.SetActive(false);    
            GameManager.Instance.CenterHallStage1();
            StartCoroutine(Deley());
        }
    }
    IEnumerator Deley()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }
}
