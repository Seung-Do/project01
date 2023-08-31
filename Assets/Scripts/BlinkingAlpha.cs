using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingAlpha : MonoBehaviour
{
    MeshRenderer renderer;
    private WaitForSeconds blinkInterval = new WaitForSeconds(0.5f);


    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        StartCoroutine(BlinkCoroutine());
    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            Color originalColor = renderer.material.color;
           
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f); // ���İ� 1 (������)
            renderer.material.color = newColor;
            yield return blinkInterval;
           
            newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // ���İ� 0 (����)
            renderer.material.color = newColor;
            yield return blinkInterval;
        }
    }
}
