using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    private Item item;


    public void AddItem(Item newItem)
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

 
    public void RemoveItem()
    {
        InventoryManager.instance.RemoveItem(item);
        Destroy(gameObject);
    }
}
