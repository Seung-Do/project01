using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicShield : MonoBehaviour, IDamage
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MONSTERMAGIC"))
        {
            other.gameObject.SetActive(false);
            print("����");
        }
    }*/

    public void getDamage(int damage)
    {
        Debug.Log("���з� ����");
    }
}
