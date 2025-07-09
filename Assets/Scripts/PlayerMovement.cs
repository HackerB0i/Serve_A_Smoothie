using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    
    private Rigidbody2D _rb;
    private Vector2 _velocity;
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        var HORIZONTAL = Input.GetAxis("Horizontal");   
        var VERTICAL = Input.GetAxis("Vertical");

        var velocity = new Vector2(HORIZONTAL, VERTICAL);
        _rb.velocity = _velocity;
    }
}
