﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTextureScrolling : MonoBehaviour
{
    private Renderer planeRenderer;
    bool gameActive = false;

    void Start()
    {
        planeRenderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.AddListener<GameEndEvent>(OnGameEndEvent);
    }

    private void OnDisable()
    {
        EventManager.Instance.RemoveListener<GameStartedEvent>(OnGameStartedEvent);
        EventManager.Instance.RemoveListener<GameEndEvent>(OnGameEndEvent);
    }

    void Update()
    {
        if (gameActive)
        {
            float offset = Time.time * (WorldConstants.Instance.WorldScrollSpeed / 5.2f);

            planeRenderer.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
        }
    }

    void OnGameStartedEvent(GameStartedEvent e)
    {
        gameActive = true;
    }

    void OnGameEndEvent(GameEndEvent e)
    {
        gameActive = false;
    }
}
