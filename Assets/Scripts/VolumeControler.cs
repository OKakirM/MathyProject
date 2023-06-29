using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControler : MonoBehaviour
{
    public AudioSource bg;

    // Start is called before the first frame update
    void Start()
    {
        FadeIn();
        bg.volume = 0f;
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(true, bg, 1f, .2f));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(true, bg, 1f, 0f));
    }

    public IEnumerator Fade(bool fadeIn, AudioSource source, float duration, float targetVolume)
    {
        if (!fadeIn)
        {
            double lengthOfSource = (double)source.clip.samples / source.clip.frequency;
            yield return new WaitForSecondsRealtime((float)(lengthOfSource - duration));
        }

        float time = 0f;
        float startVol = source.volume;
        while(time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVol, targetVolume, time / duration);
            yield return null;
        }
        yield break;
    }
}
