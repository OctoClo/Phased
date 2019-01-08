using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public RectTransform Separator;
    public RectTransform BarBackground;

    [HideInInspector]
    public float Value
    {
        get
        {
            return currentValue;
        }
        set
        {
            currentValue = Mathf.Clamp(value, 0, 100f);
            UpdateBar();
        }
    }

    Image barImage;
    float maxValue = 100f;
    float currentValue;
    float fillAmount;

    void Start()
    {
        barImage = GetComponent<Image>();
    }
    
    void UpdateBar()
    {
        fillAmount = currentValue / maxValue;
        barImage.fillAmount = fillAmount;
    }

    public void SetSeparator(float value)
    {
        Vector2 newPos = new Vector3(value * BarBackground.rect.width / maxValue, Separator.anchoredPosition.y);
        Separator.anchoredPosition = newPos;
    }
}
