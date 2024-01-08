using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public GameObject bodySegmentPrefab;
    public GameObject tailSegmentPrefab;
    private Queue<Vector2> headPositions = new Queue<Vector2>();
    public float maxSpeed = 5.0f;
    public float accelerationSpeed = 0.1f;
    private Vector2 currentVelocity = Vector2.right; // Initial velocity
    private Rigidbody2D rb;
    private List<Vector2> HeadPositions = new List<Vector2>(); // Changed from Queue to List for better indexed access
    private List<Transform> bodySegments = new List<Transform>();
    private Transform tailSegment;
    private Queue<Vector2> positionsQueue = new Queue<Vector2>();
    private Vector2 previousTailPosition;
    public int initialSize = 1;
    public int additionalPositionsPerSegment = 10; // New variable for spacing
    public float segmentSpacing = 10; // New variable for spacing
    public int tailSpacing = 10;
    //private int movementSmoothness = (int)maxSpeed;
    public int tailMovementSmoothness = 1;
    public float GameMaxSpeed = 15f;
    public int buffer = 20;
    private int movementSmoothness;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InitializeSnake();

    }

    void Update()
    {
        HandleInput();
       // DebugLogSegments();
    }
    void InitializeSnake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            GrowSnake(bodySegmentPrefab); // Pass the distance to position segments
        }
    }

    private int MaximumHeadPositions()
    {
        // Calculate the maximum number of positions based on the size of the snake
        // and the segment spacing. You might want to add a buffer for smoother movement.

        int maxPositionsBasedOnSnakeSize = bodySegments.Count * Mathf.CeilToInt(segmentSpacing);

        // Add a buffer for additional positions to ensure smooth movement.
        // The size of this buffer can be adjusted based on your game's requirements.

        return maxPositionsBasedOnSnakeSize + buffer;
    }
    void FixedUpdate()
    {
        MoveSnake();
        RotateHead();

        // Calculate the adjusted segment spacing based on speed
        //float adjustedSegmentSpacing = Mathf.Lerp(1f, 4f, maxSpeed / GameMaxSpeed);
        movementSmoothness = (int)maxSpeed+2;
        if (Time.frameCount % Mathf.RoundToInt(segmentSpacing) == 0)
        {
            HeadPositions.Add(transform.position);
        }

        if (HeadPositions.Count > MaximumHeadPositions())
        {
            HeadPositions.RemoveAt(0);
        }

        // Calculate the adjusted movement smoothness based on speed
        //int adjustedMovementSmoothness = Mathf.RoundToInt(Mathf.Lerp(1f, 5f, currentVelocity.magnitude / GameMaxSpeed));

        MoveBodySegments(movementSmoothness, segmentSpacing);
       // Debug.Log("current segmentSpaceing/Smoothness " + adjustedSegmentSpacing + "/" + adjustedMovementSmoothness);
    }

    void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector2 inputDirection = new Vector2(horizontalInput, verticalInput).normalized;

        // Accelerate in the direction of input if there's input
        if (inputDirection != Vector2.zero)
        {
            currentVelocity += inputDirection * accelerationSpeed;
            currentVelocity = Vector2.ClampMagnitude(currentVelocity, maxSpeed);
        }
        // If no input and current velocity is less than maxSpeed, continue to accelerate to maxSpeed
        else if (currentVelocity.magnitude < maxSpeed)
        {
            currentVelocity = Vector2.MoveTowards(currentVelocity, currentVelocity.normalized * maxSpeed, accelerationSpeed);
        }
        // If current velocity equals maxSpeed, maintain this velocity
        else
        {
            currentVelocity = currentVelocity.normalized * maxSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Food food = other.gameObject.GetComponent<Food>();
            if (food != null)
            {
                // Assuming XPManager is a singleton or accessible globally
                EXPManager.Instance.AddXP(food.expValue);
            }

          //  GrowSnake(); This is called upon gaining trait
            Destroy(other.gameObject); // Destroy the food
        }
    }


    void MoveSnake()
    {
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }

    void RotateHead()
    {
        float angle = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }



    public void GrowSnake(GameObject segmentPrefab)
    {
        Vector2 newSegmentPosition = (bodySegments.Count == 0) ?
            (Vector2)transform.position - currentVelocity.normalized :
            (Vector2)bodySegments[bodySegments.Count - 1].position - currentVelocity.normalized;

        GameObject newSegment = Instantiate(segmentPrefab, newSegmentPosition, Quaternion.identity);
        bodySegments.Add(newSegment.transform);

        if (bodySegments.Count == 1 && tailSegmentPrefab != null)
        {
            GameObject tail = Instantiate(tailSegmentPrefab, newSegmentPosition, Quaternion.identity);
            tailSegment = tail.transform;
        }

        // Add enough positions to the queue for the new segment
        for (int i = 0; i < additionalPositionsPerSegment; i++)
        {
            positionsQueue.Enqueue(newSegmentPosition);
        }
    }

    void MoveBodySegments(float adjustedMovementSmoothness, float adjustedSegmentSpacing)
    {
        for (int i = 0; i < bodySegments.Count; i++) // Changed from foreach to for loop for better performance
        {
            int targetIndex = Mathf.FloorToInt(i * adjustedSegmentSpacing); // Calculate the index once
            if (targetIndex < HeadPositions.Count) // Check if the target position exists
            {
                Vector2 targetPosition = HeadPositions[targetIndex]; // Direct access instead of using ElementAt
                bodySegments[i].position = Vector2.Lerp(bodySegments[i].position, targetPosition, Time.deltaTime * adjustedMovementSmoothness); // Smooth interpolation
            }
        }
    
        if (tailSegment != null)
        {
            Vector2 direction = (Vector2)tailSegment.position - previousTailPosition;
            if (direction != Vector2.zero)
            {
                tailSegment.up = direction;
            }
            previousTailPosition = tailSegment.position;
        }
        UpdateTailSegment();
    }

    void UpdateTailSegment()
    {
        if (tailSegment != null && bodySegments.Count > 1)
        {
            Transform lastBodySegment = bodySegments[0];
            Transform lastBodySegment2 = bodySegments[1];
            Vector2 directionToLastSegment = ((Vector2)lastBodySegment.position - (Vector2)lastBodySegment2.position).normalized;
            Vector2 targetPosition = (Vector2)lastBodySegment.position + directionToLastSegment * tailSpacing;

            tailSegment.position = Vector2.Lerp(tailSegment.position, targetPosition, Time.deltaTime * tailMovementSmoothness);
            tailSegment.up = directionToLastSegment;
        }
        else {
            Transform lastBodySegment = bodySegments[0];
            Vector2 directionToLastSegment = ((Vector2)rb.position - (Vector2)lastBodySegment.position ).normalized;
            Vector2 targetPosition = (Vector2)lastBodySegment.position + directionToLastSegment * tailSpacing;
            tailSegment.position = Vector2.Lerp(tailSegment.position, targetPosition, Time.deltaTime * tailMovementSmoothness);
            tailSegment.up = -directionToLastSegment;
        }

    }

    void DebugLogSegments()
    {
        for (int i = 0; i < bodySegments.Count; i++)
        {
            Vector2 segmentPosition = bodySegments[i].position;
            Debug.Log("Segment " + i + ": Position = " + segmentPosition);
        }
    }
}