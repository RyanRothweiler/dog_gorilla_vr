using UnityEngine;

public class CollectableBehaviorIdle : ICollectableBehavior
{
    private CollectableController controller;

    private const float SPIN_RATE = 100.0f;
    private const float BOB_RATE = 5.0f;
    private const float BOB_HEIGHT = 0.1f;

    public CollectableBehaviorIdle(CollectableController controller)
    {
        this.controller = controller;
    }

    public void Update()
    {
        controller.model.transform.Rotate(new Vector3(0.0f, SPIN_RATE * Time.deltaTime, 0.0f));
        controller.model.transform.localPosition = new Vector3(0.0f, Mathf.Cos(Time.time * BOB_RATE) * BOB_HEIGHT, 0.0f);
    }
}
