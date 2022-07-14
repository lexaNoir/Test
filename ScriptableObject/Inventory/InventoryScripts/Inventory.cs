using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [CreateAssetMenu(menuName = "Inventory/Inventory")]
    public class Inventory : ScriptableObject
    {
        public InventorySlot[] Slot;// = new InventorySlot();
        
        public int maxCollection = 1; // максимальное количество итемов в одной ячейке
        public ItemType[] itemTypeAdd; // тип предметов которые могут находиться в инвентаре
        
        
        [ContextMenu("Clear")]
        public void Clear()
        {
            for (int i = 0; i < Slot.Length; i++)
            {
                
                Slot[i].item = new Item();
                Slot[i].amount = 0;
                Slot[i].ItemCellId = -1;

            }
        }

    }

 
[System.Serializable]
public class InventorySlot
{
    public int ID; //ID ячейки, совпадает с номером в массиве
    public Item item; //то, что лежит в ней
    public int ItemCellId; // Id в конкретном инвентаре
    public int amount; // количество в данном месте
    public Vector2 anhoredPosition;
    public Transform transform;
    //     public Inventory beInventory; // принадлежность ячейки инвентарю
    // public bool CellStatus; //занята или нет

    public InventorySlot(InventorySlot  _slot)
    {
        ID = _slot.ID;
        ItemCellId = -1;
        item =  _slot.item;
        amount = _slot.amount;
        anhoredPosition = _slot.anhoredPosition;
        transform = _slot.transform;
    }

}
