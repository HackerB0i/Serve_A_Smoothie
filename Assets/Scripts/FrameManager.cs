using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class FrameManager : MonoBehaviour
{
    [SerializeField] private int selectedFrame;
    [SerializeField] private bool isInteracting;
    [SerializeField] private InputActionAsset inputActions;

    private List<InventoryFrame> frames = new();

    private float _changeCooldown;
    private int _interactingPlayer = -1;

    private InputActionMap pMap;
    private InputAction _playerHorizontal;

    private void Start()
    {
        frames = GetComponentsInChildren<InventoryFrame>().ToList();
        pMap = inputActions.FindActionMap($"Player");
    }

    private void Update()
    {
        for (int i = 0; i < frames.Count; i++)
        {
            if (i == selectedFrame)
            {
                frames[i].isSelected = true;
            }
            else
            {
                frames[i].isSelected = false;
            }
        }
        if (isInteracting && _playerHorizontal.IsPressed() && _changeCooldown <= 0)
        {
            selectedFrame += (int)_playerHorizontal.ReadValue<float>();
            _changeCooldown = 0.1f;
        }
        _changeCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && _interactingPlayer == -1)
        {
            isInteracting = true;
            _interactingPlayer = collision.GetComponent<PlayerDesignator>().GetPlayerNumber();
            _playerHorizontal = pMap.FindAction($"P1H");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player" && _interactingPlayer != -1)
        {
            isInteracting = false;
            _interactingPlayer = -1;
        }
    }
}
