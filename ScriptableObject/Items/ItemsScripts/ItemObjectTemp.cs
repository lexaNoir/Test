using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ItemTypetemp 
{
    Food,
    Scroll,
    Weapon,
    Armor,
    Potion,
    Money,
    Quest
}

/*public enum MagickBuff
{
    Ice,
    Cold,
    Fire,
    Water,
    Shadow
}

public enum Stats
{
    Agility,
    Strenght,
    Health,
    Stamina
}*/

[CreateAssetMenu(menuName = "Inventory/Item")]

public class ItemObjectTemp : ScriptableObject
{
    public Guid id = Guid.NewGuid();
    public string Name;
    public Sprite uiDisplay;
    public Sprite dragIcon;
    public bool stackable;
    public ItemTypetemp type;
    [TextArea(15, 20)]
    public string description;
    public int cells;
    public float coast;
  
    /*
    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
    */
    
}

/*
[System.Serializable]
public class Item
{
    public string Name;
    public Guid Id;
    public Sprite UIDisplay;
    public Sprite DragDisplayItem;
    public int Cell;
    public float Coast;
    public string Description;
    public string Type;
   
    
    public Item()
    {
        Name = "";
    }
    public Item(ItemObject item)
    {
        
        Name = item.name;
        Id = item.id;
        UIDisplay = item.uiDisplay;
        DragDisplayItem = item.dragIcon;
        Description = item.description;
        Cell = item.cells;
        Coast = item.coast;
        Type = item.type.ToString();


    }
    

    
    
   


}*/