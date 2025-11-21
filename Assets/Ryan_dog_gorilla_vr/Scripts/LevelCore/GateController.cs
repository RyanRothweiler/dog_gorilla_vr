using System.Collections;
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
    private AudioSource openSound;

    [SerializeField]
    private BoxCollider doorTrigger;

    private static readonly Vector3 DOOR_OPEN_POS = new Vector3(0.0f, -1.5f, 0.0f);

    private const float DOOR_OPEN_DUR_SEC = 15.0f;

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
        openSound.Play();

        Vector3 startPos = doorModel.transform.localPosition;
        Vector3 endPos = DOOR_OPEN_POS;
        float t = 0f;

        while (t < DOOR_OPEN_DUR_SEC)
        {
            t += Time.deltaTime;
            float lerp = Mathf.Clamp01(t / DOOR_OPEN_DUR_SEC);

            doorModel.transform.localPosition = Vector3.Lerp(startPos, endPos, lerp);

            yield return null;
        }

        // Finished opening
        doorModel.SetActive(false);
        openSound.Stop();
    }

}
