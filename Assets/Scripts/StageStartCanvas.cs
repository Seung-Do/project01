using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageStartCanvas : MonoBehaviour
{
    private int times = 0;
    [SerializeField] private GameObject text;
    [SerializeField] private int stageNumber;
    public GameObject canvas;
    void Start()
    {
        if (stageNumber == 0)
            GameManager.Instance.stage0Canvas = canvas;
        else if (stageNumber == 1)
            GameManager.Instance.stage1Canvas = canvas;
        canvas.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(OnOffCanvas());
    }
    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator OnOffCanvas()
    {
        while (times < 6)
        {
            text.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            text.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            times++;
        }
        canvas.SetActive(false);
    }
}
