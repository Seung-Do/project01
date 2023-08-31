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
           
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f); // 알파값 1 (불투명)
            renderer.material.color = newColor;
            yield return blinkInterval;
           
            newColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // 알파값 0 (투명)
            renderer.material.color = newColor;
            yield return blinkInterval;
        }
    }
}
