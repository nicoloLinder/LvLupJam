using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class CarController : MonoBehaviour
{
    [SerializeField] private bool demoMode;
    [SerializeField] private int playerNumber;
    [SerializeField] private InputAction movement;
    [SerializeField] private float speed;
    [SerializeField] private Sprite[] carSprites;

    [SerializeField] private SpriteRenderer spriteRenderer;

    // [SerializeField] private Transform livesContainer;
    [SerializeField] private Transform shadow;

    [SerializeField] private HealthContainer healthContainer;

    [SerializeField] private ParticleSystem _particleSystem;

    [SerializeField] private AudioSource movingAudio;
    [SerializeField] private AudioSource crashAudio;
    

    private int rotationIndex;

    private Rigidbody2D _rigidbody2D;

    private readonly int[] _directionArray = {60, 0, 300, 90, 0, 270, 120, 180, 240};

    private float _shadowX;

    private int Vertical { get; set; }
    private int Horizontal { get; set; }

    public int PlayerNumber => playerNumber;

    // Start is called before the first frame update
    void Awake()
    {
        movement.performed += OnMovementPerformed;
        movement.canceled += OnMovementPerformed;

        _rigidbody2D = GetComponent<Rigidbody2D>();

        _shadowX = shadow.transform.localPosition.x;
    }

    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
        if (demoMode) return;

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
        crashAudio.Play();
        switch (collision.gameObject.tag)
        {
            case "thunderdome":
                
                Dome.Instance.DomeHit();
                if (healthContainer.RemoveHealth() == false)
                {
                    if (demoMode) return;
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

    private float timePassed = 2;

    void Update()
    {
        if (demoMode)
        {
            timePassed -= Time.deltaTime;
            if (timePassed < 0)
            {
                Horizontal = Random.Range(-1, 2);
                // Horizontal = Math.Clamp(Horizontal, -1, 1);
                Vertical = Random.Range(-1, 2);
                // Vertical = Math.Clamp(Vertical, -1, 1);
                timePassed = 2;
            }

            
        }
    }


    void FixedUpdate()
    {
        _rigidbody2D.velocity += new Vector2(Horizontal, Vertical) * speed;
        if (_rigidbody2D.velocity.sqrMagnitude > 10)
        {
            if (_particleSystem.isPlaying == false)
            {
                _particleSystem.Play();
                movingAudio.Play();
            }
        }
        else
        {
            _particleSystem.Stop();
            movingAudio.Stop();
        }

        // transform.rotation =
        var index = (-Vertical + 1) * 3 + Horizontal + 1;

        if (rotationIndex == index) return;

        if (index != 4)
        {
            spriteRenderer.sprite = carSprites[index];
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, _directionArray[index]));
            spriteRenderer.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -_directionArray[index]));
        }

        shadow.transform.localPosition = Horizontal < 0 ? new Vector3(-_shadowX, 0, 0) : new Vector3(_shadowX, 0, 0);

        rotationIndex = index;
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