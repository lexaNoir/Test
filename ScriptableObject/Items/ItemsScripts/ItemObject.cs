using System;
using UnityEngine;

public enum ItemType 
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

public class ItemObject : ScriptableObject
{
    public Guid ID; //uniq Id
    public string ItemId;  
    public string Name;
    public Sprite uiDisplay; //картинка в ячейке
    public Sprite dragIcon; // картинка при перетаскивании
   // public bool stackable; // стакается ли (хз у мерчанта все стакаются)
    public ItemType type; // магический, еда и т п
    [TextArea(15, 20)]
    public string description; //описание
    public int cells; // количество ячеек
    public float coast; // стоимость


   public Item CreateItem()
   {
        Item newItem = new Item(this);
        return newItem;
    }

   
}

[System.Serializable]
public struct Item
{
    public string Name; // уникальное имя по которому можно добавить предмет в игру
    public Guid ItemId; // в базе данных для добавления предмета в игру.
    public string UniqId; // Тупо посмотреть.
    // public int ItemGameId; // ID предмета в ячейке
    public Sprite UIDisplay;
    public Sprite DragDisplayItem;
    public int Cell; // Занимаемое место
    public float Coast; // Стоимость
    public string Description;
    public string Type;

    public Item(ItemObject item)
    {
        Name = item.Name;
        UniqId = item.ID.ToString();
        ItemId = item.ID;
        UIDisplay = item.uiDisplay;
        DragDisplayItem = item.dragIcon;
        Description = item.description;
        Cell = item.cells;
        Coast = item.coast;
        Type = item.type.ToString();
    }

}
/*
[System.Serializable]
public class Item
{
   public ItemStruct _itemStruct;
    public Item(ItemObject item)
    {
        _itemStruct.Name = item.Name;
        _itemStruct.UniqId = item.ID.ToString();
        _itemStruct.UIDisplay = item.uiDisplay;
        _itemStruct.DragDisplayItem = item.dragIcon;
        _itemStruct.Description = item.description;
        _itemStruct.Cell = item.cells;
        _itemStruct.Coast = item.coast;
        _itemStruct.Type = item.type.ToString();
    }
}

public struct ItemStruct
{
    
    public string Name; // уникальное имя по которому можно добавить предмет в игру
    public Guid ItemId; // в базе данных для добавления предмета в игру.
    public string UniqId; // Тупо посмотреть.
    // public int ItemGameId; // ID предмета в ячейке
    public Sprite UIDisplay;
    public Sprite DragDisplayItem;
    public int Cell; // Занимаемое место
    public float Coast; // Стоимость
    public string Description;
    public string Type;

    
}
*/