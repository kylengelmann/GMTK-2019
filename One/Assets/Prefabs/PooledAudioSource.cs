using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledAudioSource : MonoBehaviour
{
    AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
        StartCoroutine(waitForFinish());
    }

    IEnumerator waitForFinish()
    {
        while(source.isPlaying)
        {
            yield return null;
        }
        ObjectPoolManager.ReturnPooledObject(PooledObjectType.AudioSource, gameObject);
    }
}
