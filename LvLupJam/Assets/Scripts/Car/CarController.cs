using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CarController : MonoBehaviour
{
    
    [SerializeField] private InputAction movement;
    [SerializeField] private float speed;
    [SerializeField] private Sprite[] carSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform livesContainer;

    [SerializeField] private HealthContainer healthContainer;

    private Rigidbody2D _rigidbody2D;

    private readonly int[] _directionArray = {60, 0, 300, 90, 0, 270, 120, 180, 240};

    private int Vertical { get; set; }
    private int Horizontal { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        movement.performed += OnMovementPerformed;
        movement.canceled += OnMovementPerformed;

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
        switch (collision.gameObject.tag)
        {
            case "thunderdome":
                if (healthContainer.RemoveHealth() == false)
                {
                    CarDead();
                }
                break;
            case "car":
                break;
            default: 
                break;
        }

        var collisionRelativeVelocity = collision.relativeVelocity;
        GetHit(transform.position, collision.contacts[0].point, transform.up, collisionRelativeVelocity);
    }

    void GetHit(Vector2 positon, Vector2 otherPosition, Vector2 forward, Vector2 collisionRelativeVelocity)
    {
        Vector2 direction = otherPosition - positon;

        var angle = Vector2.Angle(direction, forward);

        collisionRelativeVelocity /= 10;

        if (angle > 135)
        {
            _rigidbody2D.velocity += (-direction * 25) + collisionRelativeVelocity;
        }
        else if (angle > 45)
        {
            _rigidbody2D.velocity += (-direction * 20) + collisionRelativeVelocity;
        }
        else
        {
            _rigidbody2D.velocity += (-direction * 10) + collisionRelativeVelocity;
        }
    }

    void Update()
    {
        _rigidbody2D.velocity += new Vector2(Horizontal, Vertical) * speed;
        // transform.rotation =
        var index = (-Vertical + 1) * 3 + Horizontal + 1;

        if (index != 4)
        {
            spriteRenderer.sprite = carSprites[index];
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, _directionArray[index]));
            spriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -_directionArray[index]));
        }
    }

    private void CarDead()
    {
        MatchController.Instance.ReportCarDead(this);
        SetForDeletion();
    }

    public void SetForDeletion()
    {
        Destroy(gameObject);
    }
}