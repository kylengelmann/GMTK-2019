using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image fade;

    public IEnumerator DoFade(float time)
    {
        float timeDoing = 0f;
        while(timeDoing < time)
        {
            timeDoing += Time.deltaTime;
            fade.color = Color.Lerp(Color.clear, Color.black, timeDoing/time);
            yield return null;
        }
        fade.color = Color.black;
    }
}
