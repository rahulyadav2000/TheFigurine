using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
        PLANT
    }
    
    public void Use()
    {
        switch (type)
        {
            case Item.ItemType.PLANT:
                Player.instance.playerHealth.AddHealth((float)value);

                break;
            case Item.ItemType.LOG:
                Player.instance.arrow.IncreaseArrowAmount(value);

                break;
        }
    }
}
