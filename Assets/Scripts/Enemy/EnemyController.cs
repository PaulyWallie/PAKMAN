using UnityEngine;

/// <summary>
/// Base class for all enemies. Handles modular health, stomp detection, and patrol modes.
/// </summary>
[RequireComponent(typeof(Collider2D), typeof(Health))]
public class EnemyController : MonoBehaviour
{
    public enum PatrolMode { Linear, Waypoints, EdgeDetection, ChasePlayer, None }

    [Header("Base Settings")]
    public int contactDamage = 1;
    public float moveSpeed = 2f;

    [Header("Patrol Configuration")]
    public PatrolMode patrolMode = PatrolMode.None;

    [Header("Linear Settings (Manual Limits)")]
    [Tooltip("Distance to the left of the spawn point (relative).")]
    public float leftOffset = -2f;
    [Tooltip("Distance to the right of the spawn point (relative).")]
    public float rightOffset = 2f;
    public bool isMovingRight = true;
    protected float spawnX;

    [Header("Waypoint Settings")]
    public Vector3[] patrolPoints;
    public int currentWaypoint;
    public float pointStopThreshold = 0.2f;

    [Header("Edge & Wall Detection (Parascope)")]
    public Transform detectionPoint;
    public float wallCheckDistance = 0.2f;
    public float floorCheckDistance = 0.5f;
    public LayerMask whatIsGround;
    public float turnCooldown = 0.2f;
    protected float turnTimer;

    protected bool isDead;
    protected Collider2D enemyCollider;
    protected Health health;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    protected virtual void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        
        spawnX = transform.position.x;

        // Setup initial waypoint if empty and using Waypoints mode
        if (patrolMode == PatrolMode.Waypoints && (patrolPoints == null || patrolPoints.Length == 0))
        {
            patrolPoints = new Vector3[] { transform.position };
        }
    }

    protected virtual void OnEnable()
    {
        if (health != null)
        {
            health.OnDeath += Die;
        }
    }

    protected virtual void OnDisable()
    {
        if (health != null)
        {
            health.OnDeath -= Die;
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (turnTimer > 0)
            turnTimer -= Time.deltaTime;

        switch (patrolMode)
        {
            case PatrolMode.Linear:
                HandleLinearPatrol();
                break;
            case PatrolMode.Waypoints:
                HandleWaypointPatrol();
                break;
            case PatrolMode.EdgeDetection:
                HandleEdgeDetectionPatrol();
                break;
            case PatrolMode.ChasePlayer:
                HandleChasePlayerPatrol();
                break;
        }
    }

    protected virtual void HandleChasePlayerPatrol()
    {
        if (PlayerController.instance == null) return;

        Vector3 targetPos = PlayerController.instance.transform.position;
        Vector2 direction = (targetPos - transform.position).normalized;
        
        if (rb != null)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic && rb.gravityScale > 0)
            {
                rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = direction * moveSpeed;
            }
        }

        UpdateVisuals();
    }

    protected virtual void HandleLinearPatrol()
    {
        float velocityX = isMovingRight ? moveSpeed : -moveSpeed;
        if (rb != null) rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);

        if (isMovingRight && transform.position.x >= spawnX + rightOffset)
        {
            Flip();
        }
        else if (!isMovingRight && transform.position.x <= spawnX + leftOffset)
        {
            Flip();
        }

        UpdateVisuals();
    }

    protected virtual void HandleWaypointPatrol()
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
        
        if (rb != null)
        {
            if (rb.bodyType == RigidbodyType2D.Dynamic && rb.gravityScale > 0)
            {
                rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
            }
            else
            {
                rb.linearVelocity = velocity;
            }
        }

        UpdateVisuals();
    }

    protected virtual void HandleEdgeDetectionPatrol()
    {
        float velocityX = isMovingRight ? moveSpeed : -moveSpeed;
        if (rb != null) rb.linearVelocity = new Vector2(velocityX, rb.linearVelocity.y);

        if (turnTimer > 0)
        {
            UpdateVisuals();
            return;
        }

        if (detectionPoint == null) return;

        Vector2 forwardDir = isMovingRight ? Vector2.right : Vector2.left;
        RaycastHit2D wallHit = Physics2D.Raycast(detectionPoint.position, forwardDir, wallCheckDistance, whatIsGround);
        RaycastHit2D floorHit = Physics2D.Raycast(detectionPoint.position, Vector2.down, floorCheckDistance, whatIsGround);

        if (wallHit.collider != null || floorHit.collider == null)
        {
            Flip();
        }

        UpdateVisuals();
    }

    protected virtual void Flip()
    {
        isMovingRight = !isMovingRight;
        turnTimer = turnCooldown;

        if (rb != null) rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        if (detectionPoint != null)
        {
            detectionPoint.localPosition = new Vector3(-detectionPoint.localPosition.x, detectionPoint.localPosition.y, 0);
        }
    }

    protected virtual void UpdateVisuals()
    {
        if (sr != null && rb != null && Mathf.Abs(rb.linearVelocity.x) > 0.1f)
        {
            sr.flipX = rb.linearVelocity.x > 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        if (!collision.collider.CompareTag("Player")) return;

        // Skip damage if the hit came from above (Player handles stomp)
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                return;
            }
        }

        DamagePlayer();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health.TakeDamage(damage);
    }

    private void DamagePlayer()
    {
        if (PlayerHealthController.instance != null)
            PlayerHealthController.instance.TakeDamage(contactDamage);
        
        if (PlayerController.instance != null)
            PlayerController.instance.KnockBack(transform.position);
    }

    protected virtual void Die()
    {
        isDead = true;
        OnDeath();
        if (AudioManager.instance != null) AudioManager.instance.PlaySFX(SoundType.Explosion);
        gameObject.SetActive(false);
    }

    protected virtual void OnDeath() { }

    protected virtual void OnDrawGizmos()
    {
        if (patrolMode == PatrolMode.Linear)
        {
            Gizmos.color = Color.yellow;
            float currentX = Application.isPlaying ? spawnX : transform.position.x;
            Vector3 leftPos = new Vector3(currentX + leftOffset, transform.position.y, transform.position.z);
            Vector3 rightPos = new Vector3(currentX + rightOffset, transform.position.y, transform.position.z);
            Gizmos.DrawLine(leftPos, rightPos);
            Gizmos.DrawWireCube(leftPos, new Vector3(0.2f, 0.5f, 0));
            Gizmos.DrawWireCube(rightPos, new Vector3(0.2f, 0.5f, 0));
        }
        else if (patrolMode == PatrolMode.Waypoints && patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.cyan;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Gizmos.DrawWireSphere(patrolPoints[i], 0.2f);
                int next = (i + 1) % patrolPoints.Length;
                Gizmos.DrawLine(patrolPoints[i], patrolPoints[next]);
            }
        }
        
        if (patrolMode == PatrolMode.EdgeDetection && detectionPoint != null)
        {
            Gizmos.color = Color.red;
            Vector2 forwardDir = isMovingRight ? Vector2.right : Vector2.left;
            Gizmos.DrawLine(detectionPoint.position, (Vector2)detectionPoint.position + forwardDir * wallCheckDistance);
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(detectionPoint.position, (Vector2)detectionPoint.position + Vector2.down * floorCheckDistance);
        }
    }
}

