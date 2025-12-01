using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls a gate that opens when a player has the required collectables.
/// </summary>
public class GateController : RealtimeComponent<GateControllerSyncModel>
{
    // Types of collectables required to open the gate.
    [SerializeField] private List<CollectableType> requirements;

    [Header("Components")]

    // Sound played when the gate is moving.
    [SerializeField] private AudioSource openSound;

    // Trigger collider that detects players.
    [SerializeField] private BoxCollider doorTrigger;

    // Realtime networking component for ownership control.
    [SerializeField] public RealtimeView realtimeView;

    // Local offset for the door when fully open.
    private static readonly Vector3 DOOR_OPEN_POS = new Vector3(0f, -2f, 0f);

    // Time in seconds for the door to fully open.
    private const float DOOR_OPEN_DUR_SEC = 15f;


    protected override void OnRealtimeModelReplaced(GateControllerSyncModel previousModel, GateControllerSyncModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.isMovingDidChange -= IsMovingDidChange;
        }

        if (currentModel != null)
        {
            currentModel.isMovingDidChange += IsMovingDidChange;
        }
    }

    private void IsMovingDidChange(GateControllerSyncModel model, bool isMoving)
    {
        if (isMoving)
        {
            if (!openSound.isPlaying)
                openSound.Play();
        }
        else
        {
            openSound.Stop();
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        SpookyMapPlayer player = coll.attachedRigidbody.GetComponent<SpookyMapPlayer>();
        if (player != null)
        {
            // Check if player has all required collectables.
            if (player.ContainsAtleast(requirements))
            {
                // Consume items and disable trigger to prevent re-entry.
                player.ConsumeInventory(requirements);
                doorTrigger.enabled = false;

                // Start opening the door.
                StartCoroutine(Open());
            }
        }
    }

    private IEnumerator Open()
    {
        // Request network ownership so movement syncs across clients.
        realtimeView.RequestOwnershipOfSelfAndChildren();
        model.isMoving = true;

        Vector3 startPos = transform.position;
        Vector3 endPos = transform.position + DOOR_OPEN_POS;
        float t = 0f;

        while (t < DOOR_OPEN_DUR_SEC)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / DOOR_OPEN_DUR_SEC);

            transform.position = Vector3.Lerp(startPos, endPos, lerp);

            yield return null;
        }

        model.isMoving = false;
    }
}
