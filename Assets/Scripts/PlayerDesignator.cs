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
    
    private void Start()
    {
        var pMap = inputActions.FindActionMap($"Player");
 
        _playerHorizontal = pMap.FindAction($"P1H");
        _playerHorizontal.Enable();
        _playerVertical = pMap.FindAction($"P1V");
        _playerVertical.Enable();
    }

    public List<InputAction> GetPlayerInputActions()
    {
        return new List<InputAction>{_playerHorizontal, _playerVertical};
    }
}
