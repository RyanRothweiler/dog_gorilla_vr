using Normal.Realtime;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField]
    public CollectableType collectableType;

    [SerializeField]
    public GameObject model;

    [SerializeField]
    private Collider coll;

    [SerializeField]
    public RealtimeView realtimeView;

    private ICollectableBehavior currentBehavior;

    void Start()
    {
        currentBehavior = new CollectableBehaviorIdle(this);
    }

    void Update()
    {
        currentBehavior.Update();
    }

    private void OnCollisionEnter(Collision other)
    {
        SpookyMapPlayer player = other.gameObject.GetComponent<SpookyMapPlayer>();
        if (player != null)
        {
            player.Collect(this);
            currentBehavior = new CollectableBehaviorFollowPlayer(this, other.gameObject.transform);
        }
    }

    public void ActivatePhysics(bool state)
    {
        coll.enabled = state;
    }

    public void Consume()
    {
        // Realtime.Destroy(gameObject);
    }
}