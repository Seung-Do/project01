using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBookIce : MonoBehaviour
{
    private float rotationSpeed = 60f;
    [SerializeField] GameObject currTarget;
    [SerializeField] GameObject nextTarget;
    [SerializeField] GameObject canvas;
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
            controllManager.bookSpell[2].SetActive(true);
            controllManager.icePosible = true;

            canvas.SetActive(true);
            currTarget.SetActive(false);
            nextTarget.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
