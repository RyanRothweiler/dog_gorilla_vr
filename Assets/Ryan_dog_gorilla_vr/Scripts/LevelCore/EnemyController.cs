using Normal.Realtime;
using UnityEngine;
using Normal.GorillaTemplate;

/// <summary>
/// Controls enemy behavior: patrols by default, and chases players in view cone when detected.
/// Only the network owner updates enemy logic.
/// </summary>
public class EnemyController : MonoBehaviour
{
    // Movement speed of the enemy.
    [SerializeField] private float moveSpeed;

    // Minimum dot product between enemy forward and player direction to consider player in view.
    [SerializeField] private float viewConeDot;

    // Maximum distance to check for players.
    [SerializeField] private float enemyCheckDist;

    [Header("Components")]

    // Patrol behavior component (enabled when not chasing).
    [SerializeField] private PatrolBehavior patrolBehavior;

    // Networking component for ownership checks.
    [SerializeField] private RealtimeView realtimeView;

    // Current target player to chase.
    private SpookyMapPlayer chaseTarget;

    // Vertical offset for raycasting from enemy position.
    private const float FLOOR_OFFSET = 0.5f;

    void Update()
    {
        // Only the network owner updates enemy behavior.
        if (!realtimeView.isOwnedLocallySelf)
        {
            return;
        }

        chaseTarget = null;

        // Look for new target
        foreach (var kvp in GorillaPlayerManager.Instance.avatars)
        {
            GorillaAvatar avatar = kvp.Value;

            if (Vector3.Distance(transform.position, avatar.transform.position) < enemyCheckDist)
            {
                Vector3 origin = transform.position + Vector3.up * FLOOR_OFFSET;
                Vector3 dirToPlayer = (avatar.transform.position - origin).normalized;

                Ray ray = new Ray(origin, dirToPlayer);
                Debug.DrawRay(ray.origin, ray.direction, Color.green, 0.01f);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.rigidbody != null)
                    {
                        SpookyMapPlayer player = hit.collider.attachedRigidbody.GetComponent<SpookyMapPlayer>();
                        if (player != null)
                        {
                            // Check if player is within enemy view cone
                            float dot = Vector3.Dot(transform.forward, dirToPlayer);
                            if (dot > viewConeDot)
                            {
                                chaseTarget = player;
                            }
                        }
                    }
                }
            }
        }

        // Enable patrol if no player is detected
        patrolBehavior.enabled = (chaseTarget == null);

        // Chase player if detected
        if (chaseTarget != null)
        {
            Vector3 dir = (chaseTarget.transform.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
            transform.forward = dir;
        }
    }
}
