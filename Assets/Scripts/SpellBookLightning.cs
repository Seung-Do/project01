using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookLightning : MonoBehaviour
{
    private float rotationSpeed = 60f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PLAYER"))
        {
            ControllManager controllManager = other.gameObject.GetComponent<ControllManager>();
            controllManager.bookSpell[1].SetActive(true);
            controllManager.lightningPosible = true;
            GameManager.Instance.Boss0Kill();
            gameObject.SetActive(false);
        }
    }
    
}
