using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DragonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public Collider2D mapboundary;
    public SpriteRenderer spriteRenderer;
    public GameObject DragonSprite;
    public Animator animator; // Make sure this is assigned in the inspector
    private bool isOutOfBounds = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        ProcessInputs();
        Animate();
    }

    void FixedUpdate()
    {
        if (!isOutOfBounds)
        {
            Move();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            isOutOfBounds = true;
            StopMovement();
            rb.velocity = new Vector2(-moveDirection.x * 0.1f * moveSpeed, -moveDirection.y * 0.1f * moveSpeed);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Boundary"))
        {
            isOutOfBounds = false;
        }
    }

    void StopMovement()
    {
        rb.velocity = Vector2.zero; // This stops the dragon
                                    // Add any additional logic if needed, such as playing an animation or sound
    }

    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);

        // Flip the sprite based on direction
        if (moveDirection.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void Animate()
    {
        // Check if the dragon is moving
        bool isIDLE = moveDirection.x == 0 && moveDirection.y == 0;
        animator.SetBool("IsIDLE", isIDLE);


    }
}
