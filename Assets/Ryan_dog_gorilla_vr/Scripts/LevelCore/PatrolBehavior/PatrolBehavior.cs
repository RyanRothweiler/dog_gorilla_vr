using Normal.Realtime;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles patrolling along a path with optional idle at each point.
/// Requests ownership in Realtime to ensure movement is networked.
/// </summary>
public class PatrolBehavior : MonoBehaviour
{
    // Movement speed of the patrolling object.
    [SerializeField] private float moveSpeed;

    // Path to patrol along.
    [SerializeField] private PatrolPath patrolPath;

    [Header("Components")]
    // Networking component for ownership control.
    [SerializeField] private RealtimeView realtimeView;

    // Current index along the patrol path.
    private int pathIndex;

    // Timer for idling at a patrol point.
    private float idleTimer;

    // Flag to ensure ownership is only requested once.
    private bool didOwnership = false;

    // Distance threshold to consider that we've reached a patrol point.
    private const float CLOSE_ENOUGH = 0.25f;

    // Time to idle at each patrol point.
    private const float IDLE_DUR_SEC = 2.0f;

    void Update()
    {
        // Request ownership once when the RealtimeView is ready.
        // This could be replaced with events instead of polling.
        // This could also be replaced with ownership rules. The first player to connect owns the enemies AI.
        if (!didOwnership)
        {
            try
            {
                realtimeView.RequestOwnershipOfSelfAndChildren();
                didOwnership = true;
            }
            catch (System.Exception)
            {
                // Ignore exceptions — RealtimeView might not be fully initialized yet
            }
        }

        // Only move if there is a path and we own this object locally
        if (patrolPath.path.Count > 0 && realtimeView.isOwnedLocallySelf)
        {
            Transform pathTarget = patrolPath.path[pathIndex % patrolPath.path.Count];

            // Move toward the target if not close enough
            if (Vector3.Distance(pathTarget.position, transform.position) > CLOSE_ENOUGH)
            {
                Vector3 dir = (pathTarget.position - transform.position).normalized;
                transform.position += dir * moveSpeed * Time.deltaTime;
                transform.forward = dir;
            }
            else
            {
                // Idle at this point before moving to the next
                idleTimer += Time.deltaTime;
                if (idleTimer > IDLE_DUR_SEC)
                {
                    idleTimer = 0f;
                    pathIndex++;
                }
            }
        }
    }
}
