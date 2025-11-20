using Normal.Realtime;
using UnityEngine;
using GorillaLocomotion;

public class CollectableController : MonoBehaviour
{
    [SerializeField]
    public CollectableType collectableType;

    [SerializeField]
    public GameObject model;

    [SerializeField]
    private Collider collider;

    [SerializeField]
    private Rigidbody physicsBody;

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
        PlayerState playerState = other.gameObject.GetComponent<PlayerState>();
        if (playerState != null)
        {
            playerState.Collect(this);
            currentBehavior = new CollectableBehaviorFollowPlayer(this, other.gameObject.transform);
        }
    }

    public void ActivatePhysics(bool state)
    {
        collider.enabled = state;
        // physicsBody.isKinematic = !state;
    }

    public void Consume()
    {
        // Realtime.Destroy(gameObject);
    }
}