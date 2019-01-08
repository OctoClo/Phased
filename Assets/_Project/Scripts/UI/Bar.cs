using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public float MaxValue = 100f;
    public float SeparatorValue = 80f;
    public RectTransform Separator;
    public RectTransform BarBackground;

    Image barImage;
    float currentValue;

    void Start()
    {
        barImage = GetComponent<Image>();
        Separator.rect.Set(SeparatorValue * BarBackground.rect.width / MaxValue, Separator.rect.y, Separator.rect.width, Separator.rect.height);
    }

    public void AddValue(float value)
    {
        currentValue += value;
        UpdateBar();
    }

    void UpdateBar()
    {
        barImage.fillAmount = currentValue / MaxValue;
    }
}
