using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBook : MonoBehaviour
{
    [SerializeField] private int index;
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
            controllManager.bookSpell[index].SetActive(true);
            if (index == 1)
                controllManager.lightningPosible = true;
            else if (index == 2)
                controllManager.icePosible = true;

            gameObject.SetActive(false);
        }
    }
}
