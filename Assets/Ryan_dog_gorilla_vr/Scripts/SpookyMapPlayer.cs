using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a player in the Spooky Map.
/// Handles inventory, collecting items, and teleportation.
/// </summary>
public class SpookyMapPlayer : MonoBehaviour
{
    // Reference to the GorillaLocomotion player for movement control.
    [SerializeField] private GorillaLocomotion.Player gorillaPlayer;

    // Inventory of collected items, categorized by type.
    private Dictionary<CollectableType, List<CollectableController>> inventory = new();

    // Position to reset/teleport the player to.
    private static readonly Vector3 RESET_POS = new Vector3(0f, 1.0f, 0f);

    void Update()
    {
        // Teleport player to reset position on R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Teleport(RESET_POS));
        }
    }

    /// <summary>
    /// Adds a collectable to the player's inventory.
    /// </summary>
    public void Collect(CollectableController collectable)
    {
        if (!inventory.ContainsKey(collectable.collectableType))
        {
            inventory[collectable.collectableType] = new List<CollectableController>();
        }

        inventory[collectable.collectableType].Add(collectable);
    }

    /// <summary>
    /// Checks if player has at least the required number of each collectable type.
    /// </summary>
    public bool ContainsAtleast(List<CollectableType> requirements)
    {
        // Build counts of required items
        Dictionary<CollectableType, int> reqs = new();
        foreach (CollectableType r in requirements)
        {
            if (!reqs.ContainsKey(r))
            {
                reqs[r] = 0;
            }
            reqs[r]++;
        }

        // Check inventory against requirements
        foreach (var kvp in reqs)
        {
            if (!inventory.TryGetValue(kvp.Key, out List<CollectableController> items))
            {
                return false;
            }

            if (items.Count < kvp.Value)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Removes the specified items from inventory.
    /// </summary>
    public void ConsumeInventory(List<CollectableType> consume)
    {
        if (!ContainsAtleast(consume))
        {
            Debug.LogError("Player does not have enough inventory to consume that. Doing nothing.");
            return;
        }

        foreach (CollectableType itemType in consume)
        {
            if (!inventory.ContainsKey(itemType) || inventory[itemType].Count == 0)
            {
                Debug.LogError("Item missing from inventory. This indicates a bug in ContainsAtleast.");
                return;
            }

            List<CollectableController> typeList = inventory[itemType];
            typeList.RemoveAt(typeList.Count - 1);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        // Reset player if colliding with an enemy
        if (coll.gameObject.GetComponent<EnemyController>())
        {
            StartCoroutine(Teleport(RESET_POS));
        }
    }

    /// <summary>
    /// Teleports the player while handling GorillaLocomotion race conditions.
    /// 
    /// This is necessary because there are some race conditions in the gorilla player when teleporting the player.
    /// The player is moved by the hands, and the hands are moved by a follower sphere. 
    /// So if you teleport the player then the player just teleports back to the hands. 
    /// Ideally GorillaLocomotion.Player would be updated to support teleporting.
    /// </summary>
    private IEnumerator Teleport(Vector3 pos)
    {
        gorillaPlayer.enabled = false;
        transform.position = pos;

        yield return null;

        gorillaPlayer.InitializeValues();
        gorillaPlayer.enabled = true;
    }
}
