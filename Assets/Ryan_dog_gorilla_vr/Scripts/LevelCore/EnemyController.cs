using UnityEngine;
using Normal.GorillaTemplate;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float enemyCheckDist;

    [Header("Components")]
    [SerializeField]
    private PatrolBehavior patrolBehavior;

    private SpookyMapPlayer chaseTarget;

    private const float FLOOR_OFFSET = 0.5f;

    void Update()
    {
        // Check for new target
        foreach (var kvp in GorillaPlayerManager.Instance.avatars)
        {
            GorillaAvatar avatar = kvp.Value;

            if (Vector3.Distance(this.transform.position, avatar.transform.position) < enemyCheckDist)
            {
                Vector3 origin = this.transform.position;
                origin.y += FLOOR_OFFSET;

                Ray ray = new Ray(origin, (avatar.transform.position - origin).normalized);
                Debug.DrawRay(ray.origin, ray.direction, Color.green, 0.01f);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.rigidbody != null)
                    {
                        SpookyMapPlayer player = hit.collider.attachedRigidbody.GetComponent<SpookyMapPlayer>();
                        if (player != null)
                        {
                            chaseTarget = player;
                        }
                    }
                    else
                    {
                        chaseTarget = null;
                    }
                }
                else
                {
                    chaseTarget = null;
                }
            }
        }

        // Chase players
        patrolBehavior.enabled = (chaseTarget == null);
        if (chaseTarget != null)
        {
            Vector3 dir = (chaseTarget.transform.position - this.transform.position).normalized;

            this.transform.position = this.transform.position + (dir * moveSpeed * Time.deltaTime);
            this.transform.forward = dir;
        }
    }
}
