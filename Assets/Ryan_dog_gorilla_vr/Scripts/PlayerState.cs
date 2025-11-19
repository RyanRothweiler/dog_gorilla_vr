using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private Dictionary<CollectableType, int> inventory = new Dictionary<CollectableType, int>();

    public void Collect(CollectableType ct)
    {
        if (!inventory.ContainsKey(ct))
        {
            inventory[ct] = 0;
        }

        inventory[ct]++;
    }
}
