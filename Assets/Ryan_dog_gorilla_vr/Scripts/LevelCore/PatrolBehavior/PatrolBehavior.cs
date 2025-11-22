using Normal.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private PatrolPath patrolPath;

    [Header("Compnents")]
    [SerializeField]
    private RealtimeView realtimeView;

    private int pathIndex;
    private float idleTimer;

    private bool didOwnership = false;

    private const float CLOSE_ENOUGH = 0.25f;
    private const float IDLE_DUR_SEC = 2.0f;

    void Update()
    {
        // Ideally this would be event based so we don't need to poll. Is there an event for when the realtime model is populated?
        if (!didOwnership)
        {
            try
            {
                realtimeView.RequestOwnershipOfSelfAndChildren();
                didOwnership = true;
            }
            catch (System.Exception err)
            {

            }
        }

        if (patrolPath.path.Count > 0 && realtimeView.isOwnedLocallySelf)
        {
            Transform pathTarget = patrolPath.path[pathIndex % patrolPath.path.Count];

            if (Vector3.Distance(pathTarget.position, this.transform.position) > CLOSE_ENOUGH)
            {
                Vector3 dir = (pathTarget.position - this.transform.position).normalized;

                this.transform.position = this.transform.position + (dir * moveSpeed * Time.deltaTime);
                this.transform.forward = dir;
            }
            else
            {
                // run idle
                idleTimer += Time.deltaTime;
                if (idleTimer > IDLE_DUR_SEC)
                {
                    idleTimer = 0.0f;
                    pathIndex++;
                }
            }
        }
    }
}
