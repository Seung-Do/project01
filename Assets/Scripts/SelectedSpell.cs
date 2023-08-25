using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedSpell : MonoBehaviour
{
    ControllManager controllManager;
    public GameObject[] spell;
    private ParticleSystem particle;
    private float fadeSpeed = 1.5f;
    void Awake()
    {
        controllManager = GameObject.FindGameObjectWithTag("PLAYER").GetComponent<ControllManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        spell[controllManager.index].SetActive(true);      
        particle = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(SpellFadeOut());
    }
    public IEnumerator SpellFadeOut()
    {
        yield return new WaitForSeconds(0.5f);
        var mainModule = particle.main;
        float targetAlpha = 0f;

        while (mainModule.startColor.color.a > targetAlpha)
        {
            Color currentColor = mainModule.startColor.color;
            currentColor.a -= fadeSpeed * Time.deltaTime;
            mainModule.startColor = currentColor;
            yield return null;
        }
        Destroy(gameObject);
    }
}
