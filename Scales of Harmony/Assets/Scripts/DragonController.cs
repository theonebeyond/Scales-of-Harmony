using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    public SpriteRenderer spriteRenderer;
    public Animator animator; // Make sure this is assigned in the inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void Update()
    {
        ProcessInputs();
 //       Animate();
    }

    void FixedUpdate()
    {
        Move();
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
        bool isMoving = moveDirection.x != 0 || moveDirection.y != 0;
        if (isMoving) { animator.enabled = false;}
        else { animator.enabled = true; }

    }
}
