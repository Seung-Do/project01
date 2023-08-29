using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkeleton : MonoBehaviour, IDamage
{
    public Animator animator;
    public int hitNumber;
    //public float fadeOutTime = 2f; // 사라지는데 걸리는 시간 (초)
    //private SkinnedMeshRenderer[] renderers;

   /* private void OnEnable()
    {
        // 하위 오브젝트들의 렌더러를 찾아서 투명하게 설정
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            Color originalColor = renderer.material.color;
            Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
            renderer.material.color = transparentColor;
            Debug.Log(renderer.material.color);
        }

        
       StartCoroutine(FadeIn());
    }

    private void OnDisable()
    {       
        StopAllCoroutines();       
    }
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutTime)
        {
            // 시간에 따라 투명도 조절
            float alpha = elapsedTime / fadeOutTime;
            //Debug.Log("인"+alpha);

            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                Color originalColor = renderer.material.color;
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                renderer.material.color = newColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 나타나기 완료 후 모든 렌더러들의 투명도를 다시 1로 설정
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            Color originalColor = renderer.material.color;
            Color opaqueColor = new Color(originalColor.r, originalColor.g, originalColor.b, 1f);
            renderer.material.color = opaqueColor;
        }
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeOutTime)
        {
            // 시간에 따라 투명도 조절 (서서히 사라지도록)
            float alpha = 1f - (elapsedTime / fadeOutTime);
            //Debug.Log("아웃"+alpha);

            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                Color originalColor = renderer.material.color;
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                renderer.material.color = newColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 페이드 아웃 완료 후 몬스터 오브젝트 비활성화
        gameObject.SetActive(false);
    }*/
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public void getDamage(int damage)
    {
        animator.SetTrigger("damage");
        hitNumber++;
    }
}
