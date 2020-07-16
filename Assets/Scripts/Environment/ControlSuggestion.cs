using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSuggestion : PlayerEnterExit
{
    [SerializeField] float frequency = 1f;

    SpriteRenderer _renderer;

    IEnumerator showSuggestion;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
        showSuggestion = null;
    }

    protected override void OnPlayerEnter()
    {
        if (showSuggestion == null)
        {
            showSuggestion = Suggest();
            StartCoroutine(showSuggestion);
        }
    }

    protected override void OnPlayerExit()
    {
        StopCoroutine(showSuggestion);
        showSuggestion = null;
        _renderer.enabled = false;
    }

    private IEnumerator Suggest()
    {
        while (true)
        {
            _renderer.enabled = true;
            yield return new WaitForSeconds(1 / frequency);
            _renderer.enabled = false;
            yield return new WaitForSeconds(1 / frequency);
        }
    }

}
