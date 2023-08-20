using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public List<Item> items = new List<Item>();
    
    public Transform inventoryItem;
    public GameObject itemPrefab;

    public InventoryHandler[] inventoryHandler;

    [SerializeField] private int weight = 6;
    private void Awake()
    {
        instance = this;
    }

    public void AddItem(Item item)
    {
        if(items.Count >= weight)
        {
            return; // if the item count exceeds weight, it will return inventory is full
        }
        items.Add(item);    // adds the item in the list
        UpdatingItem();     // update the UI to see the changes
    }

    public void RemoveItem(Item item)   //remove the item from the inventory 
    {
        items.Remove(item);
    }

    public void UpdatingItem()  // updates the UI
    {
        int itemCount = items.Count;

        // Instantiate items if required
        for (int i = inventoryItem.childCount; i < itemCount; i++)
        {
            GameObject go = Instantiate(itemPrefab, inventoryItem);
            inventoryHandler = inventoryItem.GetComponentsInChildren<InventoryHandler>();
            foreach(var item in items)
            {
                var icon = go.transform.Find("Item Image").GetComponent<Image>();

                icon.sprite = item.icon;    // sets the item icon
            }
        }

        for (int i = 0; i < itemCount; i++) // update the inventory handler component with items
        {
            GameObject itemGameObject = inventoryItem.GetChild(i).gameObject;
            InventoryHandler handler = itemGameObject.GetComponent<InventoryHandler>();

            if (handler != null)
            {
                handler.AddItem(items[i]);  // adds the correspoding item to handler
            }
        }

        // destroys the extra item slots 
        for (int i = itemCount; i < inventoryItem.childCount; i++)
        {
            GameObject itemGameObject = inventoryItem.GetChild(i).gameObject;
            Destroy(itemGameObject);
        }

        SettingTheItems();
    }

    // update the item displayed in the inventory system 
    public void SettingTheItems()
    {
        inventoryHandler = inventoryItem.GetComponentsInChildren<InventoryHandler>();

        for (int i = 0; i < inventoryHandler.Length; i++)
        {
            if(i < items.Count)
            {
                inventoryHandler[i].AddItem(items[i]);
            }
            else
            {
                inventoryHandler[i].RemoveItem();
            }
        }
    }
}
