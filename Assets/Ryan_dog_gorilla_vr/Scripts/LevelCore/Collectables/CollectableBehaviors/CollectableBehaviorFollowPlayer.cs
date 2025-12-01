using UnityEngine;

/// <summary>
/// Makes a collectable smoothly follow a target transform while shrinking to a fixed size.
/// </summary>
public class CollectableBehaviorFollowPlayer : ICollectableBehavior
{
    // Controller for the collectable being animated.
    private CollectableController controller;

    // The transform the collectable follows.
    private Transform target;

    // How quickly position and scale adjust.
    private const float ANIM_RATE = 10f;

    // Vertical offset above the target.
    private const float UP_OFFSET = 0.8f;

    // Final target scale.
    private const float SCALE = 0.2f;

    public CollectableBehaviorFollowPlayer(CollectableController controller, Transform target)
    {
        this.controller = controller;
        this.target = target;

        // Take ownership for networked control.
        controller.realtimeView.RequestOwnershipOfSelfAndChildren();

        // Disable physics so motion is fully scripted.
        controller.ActivatePhysics(false);
    }

    public void Update()
    {
        Vector3 targetPos = target.position + new Vector3(0f, UP_OFFSET, 0f);

        // Move toward the target.
        controller.transform.position = Vector3.Lerp(
            controller.transform.position,
            targetPos,
            Time.deltaTime * ANIM_RATE
        );

        // Smoothly scale the collectable.
        controller.transform.localScale = Vector3.Lerp(
            controller.transform.localScale,
            new Vector3(SCALE, SCALE, SCALE),
            Time.deltaTime * ANIM_RATE
        );
    }
}
