using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    CanvasGroup canv;
    


    private void Awake()
    {
        canv = GetComponent<CanvasGroup>();
    }

    public void FadeOutImmediately()
    {
        canv.alpha = 1;
    }


    public IEnumerator FadeOut(float time)
    {
        GetComponent<Canvas>().sortingOrder = 10;
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
        GetComponent<Canvas>().sortingOrder = 0;
    }

}
