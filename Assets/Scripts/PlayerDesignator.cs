using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDesignator : MonoBehaviour
{
    [SerializeField] private int playerNumber;
    [SerializeField] private InputActionAsset inputActions;
    
    private InputAction _playerHorizontal;
    private InputAction _playerVertical;
    
    private void Awake()
    {
        var pMap = inputActions.FindActionMap($"Player");
 
        _playerHorizontal = pMap.FindAction($"P1H");
        _playerVertical = pMap.FindAction($"P1V");
    }

    public List<InputAction> GetPlayerInputActions()
    {
        return new List<InputAction>{_playerHorizontal, _playerVertical};
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }
}
