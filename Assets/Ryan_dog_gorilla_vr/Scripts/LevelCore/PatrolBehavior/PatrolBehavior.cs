using System.Collections.Generic;
using UnityEngine;

public class PatrolBehavior : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private PatrolPath patrolPath;

    private int pathIndex;
    private float idleTimer;

    private const float CLOSE_ENOUGH = 0.25f;
    private const float IDLE_DUR_SEC = 2.0f;

    void Update()
    {
        if (patrolPath.path.Count > 0)
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
