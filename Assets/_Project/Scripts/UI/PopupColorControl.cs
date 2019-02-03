using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupColorControl : MonoBehaviour
{
    public float DecreaseValue = 0.1f;
    public float DecreaseTime = 0.01f;

    TextMeshPro text;
    Color color;
    Color colorOutine;

    void Start()
    {
        text = GetComponent<TextMeshPro>();
        color = text.faceColor;
        colorOutine = text.outlineColor;
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        while (text.faceColor.a != 0)
        {
            color.a -= DecreaseValue;
            text.faceColor = color;
            colorOutine.a -= DecreaseValue;
            text.outlineColor = colorOutine;
            yield return new WaitForSecondsRealtime(DecreaseTime);
        }
    }
}
