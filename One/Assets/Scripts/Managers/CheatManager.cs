using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheatManager : MonoBehaviour
{

    void Update()
    {

        if(Debug.isDebugBuild) {
            if(Input.GetKeyDown(KeyCode.I))
            {
                
            }
            if(Input.GetKeyDown(KeyCode.K))
            {
                KillAllEnemies();
            }
        }
    }

    void ToggleInvincibility()
    {

    }

    void KillAllEnemies()
    {

    }

}
