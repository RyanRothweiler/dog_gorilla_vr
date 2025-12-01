/// <summary>
/// Defines behavior for collectables (e.g., idle, follow player).
/// </summary>
public interface ICollectableBehavior
{
    /// <summary>
    /// Called every frame to update the collectable's behavior.
    /// </summary>
    void Update();
}
