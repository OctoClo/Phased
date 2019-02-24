using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupColorControl : MonoBehaviour
{
    public float DecreaseValue = 0.1f;
    public float DecreaseTime = 0.01f;

    TextMeshProUGUI text;
    Color color;

    public void StartDisppearing()
    {
        text = GetComponent<TextMeshProUGUI>();
        color = text.faceColor;
        StartCoroutine(Disappear());
    }

    IEnumerator Disappear()
    {
        while (text.faceColor.a != 0)
        {
            color.a -= DecreaseValue;
            text.faceColor = color;
            yield return new WaitForSecondsRealtime(DecreaseTime);
        }
    }
}
