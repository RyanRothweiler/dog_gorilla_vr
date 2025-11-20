using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Dictionary<CollectableType, List<CollectableController>> inventory = new Dictionary<CollectableType, List<CollectableController>>();

    public void Collect(CollectableController collectable)
    {
        if (!inventory.ContainsKey(collectable.collectableType))
        {
            inventory[collectable.collectableType] = new List<CollectableController>();
        }
        inventory[collectable.collectableType].Add(collectable);
    }

    public bool ContainsAtleast(List<CollectableType> requirements)
    {
        // Build requirement counts
        Dictionary<CollectableType, int> reqs = new Dictionary<CollectableType, int>();
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
            CollectableType type = kvp.Key;
            int requiredCount = kvp.Value;

            // If we don't even have this type, fail immediately
            if (!inventory.TryGetValue(type, out List<CollectableController> item))
            {
                return false;
            }

            // Not enough of the required type
            if (item.Count < requiredCount)
            {
                return false;
            }
        }

        return true;
    }

    public void ConsumeInventory(List<CollectableType> consume)
    {
        if (!ContainsAtleast(consume))
        {
            Debug.LogError("Player does not have enough inventory to consume that. Doing nothing.");
            return;
        }

        foreach (CollectableType itemRemoving in consume)
        {
            if (!inventory.ContainsKey(itemRemoving) || inventory[itemRemoving].Count == 0)
            {
                Debug.LogError("Item missing from inventory. This means there is a bug in the ContainsAtLeast call.");
                return;
            }

            List<CollectableController> typeList = inventory[itemRemoving];
            // typeList[typeList.Count - 1].Consume();
            inventory[itemRemoving].RemoveAt(typeList.Count - 1);
        }
    }
}
