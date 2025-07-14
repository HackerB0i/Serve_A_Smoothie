using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CuttingMinigame : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private float hitWindow;
    [SerializeField] private Image marker;

    private float difference;
    private InputActionMap m_PMap;
    private Vector2 _defaultPosition;
    private bool _started = false;

    private void Start()
    {
        _defaultPosition = marker.rectTransform.anchoredPosition;
    }
    
    public void StartGame()
    {
        _started = true;
        marker.rectTransform.anchoredPosition = _defaultPosition;   
        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        yield return new WaitForSeconds(duration);
        _started = false;

    }

    private void Update()
    {
        if (_started)
        {
            marker.rectTransform.anchoredPosition += new Vector2(282/duration, 0) * Time.deltaTime;
        }
    }
}
