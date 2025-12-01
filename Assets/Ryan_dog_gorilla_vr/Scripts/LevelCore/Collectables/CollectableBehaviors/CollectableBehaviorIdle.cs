using UnityEngine;

/// <summary>
/// Idle behavior that spins and bobs a collectable for visual effect.
/// </summary>
public class CollectableBehaviorIdle : ICollectableBehavior
{
    // Controller for the collectable.
    private CollectableController controller;

    // Rotation speed in degrees per second.
    private const float SPIN_RATE = 100f;

    // Bobbing frequency.
    private const float BOB_RATE = 5f;

    // Bobbing amplitude.
    private const float BOB_HEIGHT = 0.1f;

    public CollectableBehaviorIdle(CollectableController controller)
    {
        this.controller = controller;
    }

    public void Update()
    {
        // Spin the model.
        controller.model.transform.Rotate(new Vector3(0f, SPIN_RATE * Time.deltaTime, 0f));

        // Bob up and down.
        controller.model.transform.localPosition =
            new Vector3(0f, Mathf.Cos(Time.time * BOB_RATE) * BOB_HEIGHT, 0f);
    }
}
