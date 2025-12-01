using Normal.Realtime;
using UnityEngine;

/// <summary>
/// Controls a collectable’s behavior, physics, and interaction with players.
/// </summary>
public class CollectableController : MonoBehaviour
{
    // The type/category of this collectable.
    [SerializeField] public CollectableType collectableType;

    // Visual model for the collectable.
    [SerializeField] public GameObject model;

    // Physics collider used for pickup detection.
    [SerializeField] private Collider coll;

    // Realtime networking component for ownership control.
    [SerializeField] public RealtimeView realtimeView;

    // Sound played when the collectable is picked up.
    [SerializeField] private AudioSource collectSound;

    // Current behavior state (idle, follow, etc.).
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
            // Tell the player they collected this item.
            player.Collect(this);

            // Switch to follow behavior.
            currentBehavior = new CollectableBehaviorFollowPlayer(this, other.transform);

            // Play pickup sound.
            if (collectSound != null)
            {
                collectSound.Play();
            }
        }
    }

    public void ActivatePhysics(bool state)
    {
        coll.enabled = state;
    }
}
