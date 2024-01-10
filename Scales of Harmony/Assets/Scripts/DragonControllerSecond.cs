using UnityEngine;

public class DragonControllerSecond : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component from the dragon GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        // Check for input and calculate movement direction
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = 1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = 1f;
        }

        // Move the dragon
        Vector3 moveDirection = new Vector3(moveX, moveY).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        // Flip the dragon sprite using SpriteRenderer
        if (moveX != 0)
        {
            spriteRenderer.flipX = moveX < 0;
            transform.eulerAngles = new Vector3(0, 0, 0); // Reset rotation
        }
        else if (moveY > 0)
        {
            // Face upwards
            transform.eulerAngles = new Vector3(0, 0, 90);
            spriteRenderer.flipX = false; // Reset flip
        }
        else if (moveY < 0)
        {
            // Face downwards
            transform.eulerAngles = new Vector3(0, 0, -90);
            spriteRenderer.flipX = false; // Reset flip
        }
    }
}
