using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines a path for patrolling objects.
/// Draws patrol points and connecting lines in the editor using gizmos.
/// </summary>
public class PatrolPath : MonoBehaviour
{
    // List of transforms representing the patrol points.
    public List<Transform> path;

    void OnDrawGizmos()
    {
        if (path == null || path.Count == 0)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        for (int i = 0; i < path.Count; i++)
        {
            Transform current = path[i];
            if (current == null)
            {
                continue;
            }

            // Draw a sphere at the patrol point
            Gizmos.DrawSphere(current.position, 0.1f);

            // Draw a line to the next point (loop back to first if at end)
            Transform next = path[(i + 1) % path.Count];
            if (next != null)
            {
                Gizmos.DrawLine(current.position, next.position);
            }
        }
    }
}
