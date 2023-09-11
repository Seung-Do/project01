using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCanvas : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject choicePanel;
    [SerializeField] private GameObject startText;
    void Start()
    {
        StartCoroutine(OnOffText());
        choicePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.Instance.playerTr.position = new Vector3(0, 0.12f, -10f);
        GameManager.Instance.playerTr.rotation = Quaternion.identity;
    }

    private IEnumerator OnOffText()
    {
        while (true)
        {
            startText.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            startText.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void TriggerPanel()
    {
        choicePanel.SetActive(true );
        startPanel.SetActive(false);
    }
}
