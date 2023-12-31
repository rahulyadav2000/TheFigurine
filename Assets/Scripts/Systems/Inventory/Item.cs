using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item/Create An Item")]
public class Item : ScriptableObject
{
    public int id;
    public String itemName;
    public Sprite icon;
    public int value;
    public ItemType type;
    public enum ItemType
    {
        LOG,
        PLANT,
        MEAT
    }
    
    public void Use() // it does the task assigned to the items 
    {
        switch (type)
        {
            case Item.ItemType.PLANT:
                Player.instance.playerHealth.AddHealth((float)value);
                break;

            case Item.ItemType.MEAT:
                Player.instance.playerHealth.AddHealth((float)value);
                break;

            case Item.ItemType.LOG:
                Player.instance.arrow.IncreaseArrowAmount(value);
                break;
        }
    }
}
