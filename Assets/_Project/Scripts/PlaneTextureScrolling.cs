using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTextureScrolling : MonoBehaviour
{ // Scroll main texture based on time

    public float ScrollSpeed = 0.5f;

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        float offset = Time.time * ScrollSpeed;

        renderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}
