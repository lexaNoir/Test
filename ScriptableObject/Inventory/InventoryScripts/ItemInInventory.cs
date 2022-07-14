using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class  ItemInInventory : MonoBehaviour
{
   /*public Sprite spriteInCell; // Sprite в инвентаре
   public Sprite spriteInGrag; // Sprite при перетаскивании*/
   // public  Item item; // Сам предмет
    public InventorySlot slot; // Инфо о ячейке, где был 
    public Inventory Inventory; // Инфо о инвентаре, где был
   
   public TMP_Text text; // Инфо о количестве, если больше одного (Компонент TMP_Text в префабе)
   public RectTransform RTransform; // Компонент RectTransform в префабе
   public Image image; // Компонент спрайта в префабе


  
}
