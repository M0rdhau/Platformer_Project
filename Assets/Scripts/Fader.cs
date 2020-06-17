using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    CanvasGroup canv;
    


    private void Start()
    {
        canv = GetComponent<CanvasGroup>();
    }


    public IEnumerator FadeOut(float time)
    {
        while (canv.alpha < 1)
        {
            canv.alpha += Time.deltaTime / time;
            yield return null;
        }
    }

    public IEnumerator FadeIn(float time)
    {
        while (canv.alpha > 0)
        {
            canv.alpha -= Time.deltaTime / time;
            yield return null;
        }
    }

}
