using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowFps : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _fpsText;
    private int _fps;

    private void Start()
    {
        _fpsText = GetComponent<TextMeshProUGUI>();
        _fps = 0;

        StartCoroutine(ShowFPS());

    }

    private void OnDisable()
    {
        StopCoroutine(ShowFPS());
    }

    private void OnEnable()
    {
        StartCoroutine(ShowFPS());
    }

    IEnumerator ShowFPS()
    {
        while (true)
        {

            _fps = (int)(1f / Time.unscaledDeltaTime);
            _fpsText.text = _fps.ToString();

            yield return new WaitForSeconds(0.5f);
        }
    }
}
