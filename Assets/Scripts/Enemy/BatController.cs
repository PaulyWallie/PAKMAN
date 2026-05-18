using UnityEngine;

/// <summary>
/// Specific controller for Bats. Inherits patrol and chase logic from EnemyController.
/// </summary>
public class BatController : EnemyController
{
    private bool hasDetectedPlayer;

    protected override void Update()
    {
        // Only move if player has been detected (was visible once)
        if (!hasDetectedPlayer) return;

        base.Update();
    }

    private void OnBecameVisible()
    {
        hasDetectedPlayer = true;
        // Default to ChasePlayer if no other mode is set
        if (patrolMode == PatrolMode.None)
        {
            patrolMode = PatrolMode.ChasePlayer;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hasDetectedPlayer = false;
    }
}

