using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private float movementSpeed;
    
    private Rigidbody2D _rb;
    private PlayerDesignator _designator;
    
    private Vector2 _velocity;

    private InputAction _horizontal;
    private InputAction _vertical;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _designator = GetComponent<PlayerDesignator>();

        var inputActions = _designator.GetPlayerInputActions();
        print(inputActions);
        _horizontal = inputActions[0];
        _vertical = inputActions[1];
        _horizontal.Enable();
        _vertical.Enable();
    }
    
    private void FixedUpdate()
    {
        _velocity = new Vector2(_horizontal.ReadValue<float>(), _vertical.ReadValue<float>());
        _velocity.Normalize();
        _rb.velocity = _velocity * movementSpeed;
    }
}
