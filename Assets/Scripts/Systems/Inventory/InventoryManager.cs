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

    public void Update()
    {

    }
    public void AddItem(Item item)
    {
        if(items.Count >= weight)
        {
            return;
        }
        items.Add(item);
        UpdatingItem();
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public void UpdatingItem()
    {
        int itemCount = items.Count;

        // Instantiate additional items if needed
        for (int i = inventoryItem.childCount; i < itemCount; i++)
        {
            GameObject go = Instantiate(itemPrefab, inventoryItem);
            inventoryHandler = inventoryItem.GetComponentsInChildren<InventoryHandler>();
            foreach(var item in items)
            {
                var icon = go.transform.Find("Item Image").GetComponent<Image>();

                icon.sprite = item.icon;
            }
        }

        for (int i = 0; i < itemCount; i++)
        {
            GameObject itemGameObject = inventoryItem.GetChild(i).gameObject;
            InventoryHandler handler = itemGameObject.GetComponent<InventoryHandler>();

            if (handler != null)
            {
                handler.AddItem(items[i]);
            }
        }

        for (int i = itemCount; i < inventoryItem.childCount; i++)
        {
            GameObject itemGameObject = inventoryItem.GetChild(i).gameObject;
            Destroy(itemGameObject);
        }

        SettingTheItems();

        Debug.Log("Inventory Item Child: " + inventoryItem.childCount);
        Debug.Log("Item Count: " + itemCount);

    }

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
