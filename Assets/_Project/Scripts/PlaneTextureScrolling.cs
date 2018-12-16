using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTextureScrolling : MonoBehaviour
{
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * WorldConstants.Instance.WorldScrollSpeed;

        renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
