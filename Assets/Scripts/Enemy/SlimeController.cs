using UnityEngine;

/// <summary>
/// Waypoint-based patrol system for enemies. Inherits from EnemyController.
/// Supports multiple points for both ground and flying enemies.
/// </summary>
using UnityEngine;

/// <summary>
/// Waypoint-based patrol system for enemies. Inherits from EnemyController.
/// Supports Linear (Manual Limits) and Waypoint paths.
/// </summary>
public class SlimeController : EnemyController
{
    public enum PatrolMode { Linear, Waypoints }

    [Header("Patrol Mode")]
    public PatrolMode patrolMode = PatrolMode.Linear;

    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    
    [Header("Linear Settings (Manual Limits)")]
    public float leftLimit;
    public float rightLimit;
    public bool isMovingRight = true;

    [Header("Waypoint Settings")]
    public Vector3[] patrolPoints;
    public int currentWaypoint;
    public float pointStopThreshold = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        // Setup initial waypoint if empty and using Waypoints mode
        if (patrolMode == PatrolMode.Waypoints && (patrolPoints == null || patrolPoints.Length == 0))
        {
            patrolPoints = new Vector3[] { transform.position };
        }
    }

    private void Update()
    {
        if (isDead) return;

        if (patrolMode == PatrolMode.Linear)
            HandleLinearPatrol();
        else
            HandleWaypointPatrol();
    }

    private void HandleLinearPatrol()
    {
        float velocityX = isMovingRight ? moveSpeed : -moveSpeed;
        rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);

        if (isMovingRight && transform.position.x >= rightLimit)
        {
            isMovingRight = false;
        }
        else if (!isMovingRight && transform.position.x <= leftLimit)
        {
            isMovingRight = true;
        }

        if (sr != null && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            sr.flipX = rb.linearVelocity.x > 0;
        }
    }

    private void HandleWaypointPatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Vector3 targetPoint = patrolPoints[currentWaypoint];
        Vector3 diff = targetPoint - transform.position;
        Vector2 direction = new Vector2(diff.x, diff.y);
        float distance = direction.magnitude;

        if (distance < pointStopThreshold)
        {
            currentWaypoint++;
            if (currentWaypoint >= patrolPoints.Length)
            {
                currentWaypoint = 0;
            }
            return;
        }

        Vector2 velocity = direction.normalized * moveSpeed;
        
        if (rb.bodyType == RigidbodyType2D.Dynamic && rb.gravityScale > 0)
        {
            rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = velocity;
        }

        if (sr != null && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            sr.flipX = rb.linearVelocity.x > 0;
        }
    }

    private void OnDrawGizmos()
    {
        if (patrolMode == PatrolMode.Linear)
        {
            Gizmos.color = Color.yellow;
            Vector3 leftPos = new Vector3(leftLimit, transform.position.y, transform.position.z);
            Vector3 rightPos = new Vector3(rightLimit, transform.position.y, transform.position.z);
            Gizmos.DrawLine(leftPos, rightPos);
            Gizmos.DrawWireCube(leftPos, new Vector3(0.2f, 0.5f, 0));
            Gizmos.DrawWireCube(rightPos, new Vector3(0.2f, 0.5f, 0));
        }
        else if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(patrolPoints[i], 0.2f);
                int next = (i + 1) % patrolPoints.Length;
                Gizmos.DrawLine(patrolPoints[i], patrolPoints[next]);
            }
        }
    }
}
