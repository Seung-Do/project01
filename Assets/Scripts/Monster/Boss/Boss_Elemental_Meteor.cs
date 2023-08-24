using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GestureCompletionData;

public class Boss_Elemental_Meteor : MonoBehaviour
{
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    Vector3 rotationOffset = new Vector3(0, 0, 0);
    public float Offset = 0;
    ParticleSystem part;
    public GameObject hit;

    private void Start()
    {
        part = GetComponent<ParticleSystem>();
        StartCoroutine(off());
    }
    private void OnParticleCollision(GameObject other)
    {
        //GameObject hit = GameManager.Instance.poolManager[1].Get(8);
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            var instance = Instantiate(hit, collisionEvents[i].intersection + collisionEvents[i].normal * Offset, new Quaternion()) as GameObject;
            instance.transform.rotation *= Quaternion.Euler(rotationOffset);
            //instance.transform.parent = transform;
        }
    }

    IEnumerator off()
    {
        yield return new WaitForSeconds(15);
        gameObject.SetActive(false);
    }
}
