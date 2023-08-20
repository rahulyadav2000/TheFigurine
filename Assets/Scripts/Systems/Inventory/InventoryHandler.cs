using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private Item item;

    public void AddItem(Item newItem) // asigns the value of the item
    {
        if (newItem != null)
        {
            item = newItem;
        }
    }
    public void UseItem()
    {
        if(item != null)
        {
            item.Use();
        }

        RemoveItem();
    }

 
    public void RemoveItem()    // removes the item from the list and destroy it from the inventory system
    {
        InventoryManager.instance.RemoveItem(item);
        Destroy(gameObject);
    }
}
