using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkeleton : MonoBehaviour, IDamage
{
    public Animator animator;
    public int hitNumber;
    //public float fadeOutTime = 2f; // ������µ� �ɸ��� �ð� (��)
    //private SkinnedMeshRenderer[] renderers;

   /* private void OnEnable()
    {
        // ���� ������Ʈ���� �������� ã�Ƽ� �����ϰ� ����
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
            // �ð��� ���� ���� ����
            float alpha = elapsedTime / fadeOutTime;
            //Debug.Log("��"+alpha);

            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                Color originalColor = renderer.material.color;
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                renderer.material.color = newColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��Ÿ���� �Ϸ� �� ��� ���������� ������ �ٽ� 1�� ����
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
            // �ð��� ���� ���� ���� (������ ���������)
            float alpha = 1f - (elapsedTime / fadeOutTime);
            //Debug.Log("�ƿ�"+alpha);

            foreach (SkinnedMeshRenderer renderer in renderers)
            {
                Color originalColor = renderer.material.color;
                Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                renderer.material.color = newColor;
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���̵� �ƿ� �Ϸ� �� ���� ������Ʈ ��Ȱ��ȭ
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
