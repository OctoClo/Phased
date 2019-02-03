using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public Animator Animator;
    public TextMeshPro Text;

    void Awake()
    {
        // Get animation (first in array)
        AnimatorClipInfo[] clipInfo = Animator.GetCurrentAnimatorClipInfo(0);

        // Destroy popup text after the end of animation
        Destroy(gameObject, clipInfo[0].clip.length);
    }

    public void SetText(string text)
    {
        Text.text = text;
    }

    public void SetColors(Color color, Color colorOutline)
    {
        Text.faceColor = color;
        Text.outlineColor = colorOutline;
    }
}
