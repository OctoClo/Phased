using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    public Animator Animator;
    public TextMeshProUGUI Text;

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

    public void SetColor(Color color)
    {
        Text.faceColor = color;
    }
}
