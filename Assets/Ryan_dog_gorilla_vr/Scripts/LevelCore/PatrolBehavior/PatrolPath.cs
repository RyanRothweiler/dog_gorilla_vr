using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    public List<Transform> path;

    void OnDrawGizmos()
    {
        if (path != null)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform trans in path)
            {
                if (trans != null)
                {
                    Gizmos.DrawSphere(trans.position, 0.1f);
                }
            }
        }
    }
}
