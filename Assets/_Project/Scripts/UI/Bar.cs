using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public RectTransform BarBackground;
    public Image BarImage;
    public RectTransform BarSeparator;
    public Image BarImageMirror;
    public RectTransform BarSeparatorMirror;

    Animator animationController;

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

    void Start()
    {
        animationController = GetComponent<Animator>();
    }
    
    float maxValue = 100f;
    float currentValue;
    float fillAmount;
    
    void UpdateBar()
    {
        fillAmount = currentValue / maxValue;
        BarImage.fillAmount = fillAmount;
        BarImageMirror.fillAmount = fillAmount;
    }

    public void SetSeparator(float value)
    {
        Vector2 newPos = new Vector3(((100 - value) * BarBackground.rect.width / maxValue) / 2.0f, BarSeparator.anchoredPosition.y);
        BarSeparator.anchoredPosition = newPos;
        newPos.x *= -1.0f;
        BarSeparatorMirror.anchoredPosition = newPos;
    }

    public void SetPhasingState(bool phasing)
    {
        //animationController.SetBool("Phasing", phasing);
    }

    public void SetPhasedState(bool phased)
    {
        //animationController.SetBool("Phased", phased);
    }
}
