using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : RealtimeComponent<GateControllerSyncModel>
{

    [SerializeField]
    private List<CollectableType> requirements;

    [Header("Components")]
    [SerializeField]
    private AudioSource openSound;

    [SerializeField]
    private BoxCollider doorTrigger;

    [SerializeField]
    public RealtimeView realtimeView;

    private static readonly Vector3 DOOR_OPEN_POS = new Vector3(0.0f, -2.0f, 0.0f);

    private const float DOOR_OPEN_DUR_SEC = 15.0f;

    public void Update()
    {
        // Can use normal property sync change events to clean this up
        if (model != null && model.isMoving)
        {
            if (!openSound.isPlaying)
            {
                openSound.Play();
            }
        }
        else
        {
            openSound.Stop();
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        PlayerState playerState = coll.attachedRigidbody.GetComponent<PlayerState>();
        if (playerState != null)
        {
            if (playerState.ContainsAtleast(requirements))
            {
                playerState.ConsumeInventory(requirements);
                doorTrigger.enabled = false;
                StartCoroutine(Open());
            }
        }
    }

    private IEnumerator Open()
    {
        realtimeView.RequestOwnershipOfSelfAndChildren();
        model.isMoving = true;

        Vector3 startPos = this.transform.position;
        Vector3 endPos = this.transform.position + DOOR_OPEN_POS;
        float t = 0f;

        while (t < DOOR_OPEN_DUR_SEC)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / DOOR_OPEN_DUR_SEC);

            this.transform.position = Vector3.Lerp(startPos, endPos, lerp);

            yield return null;
        }

        model.isMoving = false;
    }

}
