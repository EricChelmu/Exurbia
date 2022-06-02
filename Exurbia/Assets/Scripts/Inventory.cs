using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    //fixed amount of slots
    private const int SLOTS = 9;
    //list of the items of the inventory
    private List<IInventoryItem> mItems = new List<IInventoryItem>();
    //way to add items to the list
    public event EventHandler<InventoryEventArgs> ItemAdded;

    public void AddItem(IInventoryItem item)
    {
        if(mItems.Count < SLOTS)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                //when the event is activated the items will be added and all the scripts will know
                collider.enabled = false;

                mItems.Add(item);

                item.OnPickup();

                if (ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventArgs(item));
                }
            }
        }
    }
}
