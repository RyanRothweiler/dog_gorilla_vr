using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SpookyMapPlayer : MonoBehaviour
{

    [SerializeField]
    private GorillaLocomotion.Player gorillaPlayer;

    private Dictionary<CollectableType, List<CollectableController>> inventory = new Dictionary<CollectableType, List<CollectableController>>();

    private static readonly Vector3 RESET_POS = new Vector3(0.0f, 1.0f, 0.0f);

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Teleport(RESET_POS));
        }
    }

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

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.GetComponent<EnemyController>())
        {
            StartCoroutine(Teleport(RESET_POS));
        }
    }

    private IEnumerator Teleport(Vector3 pos)
    {
        gorillaPlayer.enabled = false;
        this.transform.position = pos;

        yield return null;

        gorillaPlayer.InitializeValues();
        gorillaPlayer.enabled = true;
    }
}