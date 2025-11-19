using UnityEngine;

public class CollectableBehaviorFollowPlayer : ICollectableBehavior
{
    private CollectableController controller;

    private Transform target;

    private const float ANIM_RATE = 5.0f;

    private const float UP_OFFSET = 0.4f;

    private const float SCALE = 0.2f;

    public CollectableBehaviorFollowPlayer(CollectableController controller, Transform target)
    {
        this.controller = controller;
        this.target = target;

        controller.ActivatePhysics(false);
    }

    public void Update()
    {
        Vector3 targetPos = target.position + new Vector3(0.0f, UP_OFFSET, 0.0f);

        controller.gameObject.transform.position = Vector3.Lerp(
            controller.gameObject.transform.position,
            targetPos,
            Time.deltaTime * ANIM_RATE
        );

        controller.gameObject.transform.localScale = Vector3.Lerp(
            controller.gameObject.transform.localScale,
            new Vector3(SCALE, SCALE, SCALE),
            Time.deltaTime * ANIM_RATE
        );
    }
}
