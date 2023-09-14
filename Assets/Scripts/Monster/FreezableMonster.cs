using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezableMonster : MonoBehaviour
{
    public bool isFreeze ;

    public void IFreeze()
    {
        if (isFreeze) isFreeze = false;
        else isFreeze = true;
    }

}
