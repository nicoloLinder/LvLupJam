using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class CarController : MonoBehaviour
{
    [SerializeField] private InputAction movement;
    [SerializeField] private float speed;
    [SerializeField] private Sprite[] carSprites;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;

    private int Vertical { get; set; }
    private int Horizontal { get; set; }
    
    private 

    // Start is called before the first frame update
    void Awake()
    {
        movement.performed += OnMovementPerformed;
        movement.canceled += OnMovementPerformed;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();

        Horizontal = Mathf.RoundToInt(direction.x);
        Vertical = Mathf.RoundToInt(direction.y);
    }

    private void OnDisable()
    {
        movement.Disable();
    }

    private void OnEnable()
    {
        movement.Enable();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.DrawLine(transform.position, collision.transform.position, Color.green, 10);
        Debug.Log("hit");
    }

    void Update()
    {
        _rigidbody2D.velocity += new Vector2(Horizontal, Vertical) * speed;
        
        var index = (-Vertical + 1) * 3 + Horizontal + 1;
        
        if (index != 4)
        {
            _spriteRenderer.sprite = carSprites[index];
        }
    }
}