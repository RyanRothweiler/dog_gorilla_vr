using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{

    [SerializeField]
    private List<CollectableType> requirements;

    [Header("Components")]
    [SerializeField]
    private GameObject doorModel;

    [SerializeField]
    private BoxCollider doorCollider;

    void OnTriggerEnter(Collider coll)
    {
        PlayerState playerState = coll.attachedRigidbody.GetComponent<PlayerState>();
        if (playerState != null)
        {
            if (playerState.ContainsAtleast(requirements))
            {
                playerState.ConsumeInventory(requirements);

                doorModel.SetActive(false);
                doorCollider.enabled = false;
            }
        }
    }
}
