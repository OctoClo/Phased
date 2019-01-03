using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTextureScrolling : MonoBehaviour
{
    private Renderer planeRenderer;

    void Start()
    {
        planeRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * WorldConstants.Instance.WorldScrollSpeed;

        planeRenderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
