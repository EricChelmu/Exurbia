using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInventoryItem
{
    string Name { get; }
    Sprite Image { get; }
    void OnPickup();

}


//inventory will make events happen this is why we need InventoryEventArgs which you use to check for events
//also make sure you don't forget the n in inventory...
public class InventoryEventArgs : EventArgs
{
    public InventoryEventArgs(IInventoryItem item)
    {
        Item = item;
    }

    public IInventoryItem Item;
}


