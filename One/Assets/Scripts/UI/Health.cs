using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public Image bar;

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.HasStartedLevel) return;
        bar.fillAmount = GameManager.Player.health;
    }
}
