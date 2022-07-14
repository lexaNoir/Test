using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Mono.CSharp;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;


public class InventoryLogic : MonoBehaviour
{
        public InventoryFunction operation;
        private List<Inventory> inventoryList = new List<Inventory>();
        private GameObject itemOnMouse;
        public GameObject Bullet;

        /*ObjectPool<GameObject> _pool = new ObjectPool<GameObject>(CreateItemObject, 
            (obj) => obj.SetActive(true), 
            (obj) => obj.SetActive(false), 
            (obj) => Destroy(obj), false, 10, 100);*/


/// <summary>
/// Подписка на открытие инвентаря
/// </summary>
        private void Awake()
        {
            PlayerInventoryGenerate.InventoryOpen  += OpenInventory;
            
        }



/// <summary>
/// Визуальное наполнение инвентаря
/// </summary>
/// <param name="inventory"> собственно инвентарь, с которым работаем</param>
/// <param name="slotUI"> словарь, где ячейка инвентаря привязана к визуалке</param>
        public void OpenInventory(Inventory inventory, List<GameObject> slotUI)
        {
            foreach (GameObject _slot in slotUI)
            {
                if (inventory.Slot[int.Parse(_slot.name)].ItemCellId > -1)
                {
                   operation.ItemCreate(_slot, inventory);
                }
            }
        }


/// <summary>
/// Действие происходит после отпускания кнопки мыши или убрания пальца с экрана
/// </summary>
/// <param name="_ItemInventory"> </param>

public void OnDragEnd( ItemInInventory dragStartItemComponent)
{
    //предмет, который тянем
   // ItemInInventory DragItem = _ItemInventory.GetComponent<ItemInInventory>();
   
    List<RaycastResult> _object = IsPointerOverUIObject();
    string findString = null;
    if (_object.Count > 1)
    {
        /* Regular { */
        // регулярка для определения пустого слота
        Regex _cellNum = new Regex(@"^[0-9]");
        findString = _object[0].gameObject.name;
        Match cellNum = _cellNum.Match(findString);

        // регулярка мышь над предметом
        Regex _cellItem = new Regex(@"\bitemInInventory\b");
        findString = _object[0].gameObject.name;
        Match cellItem = _cellItem.Match(findString);

        //Регулярка мышь количеством предметов (над предметом)
        Regex _cellText = new Regex(@"^Text\b");
        findString = _object[0].gameObject.name;
        Match cellText = _cellText.Match(findString);
        /* End Regular } */

        
        
        //Перемещение в пустой слот
        if (cellNum.Success)
        {
            Inventory onDragEndInventory = _object[1].gameObject.GetComponent<PlayerInventoryGenerate>()._inventory;
            GameObject _slot = _object[0].gameObject;
            
            //ПРОВЕРКА НА РАЗРЕШЕННОСТЬ ИТЕМА В ИНВЕНТАРЕ
            if (onDragEndInventory.itemTypeAdd.Length > 0)
            {
                bool ishBin = false;
                for (int i = 0; i < onDragEndInventory.itemTypeAdd.Length; i++)
                {
                    if (onDragEndInventory.itemTypeAdd[i].ToString() == dragStartItemComponent.slot.item.Type)
                        ishBin = true;
                }

                if (!ishBin) 
                    return;
            }
            
            
            //СПЕРВА РАЗДЕЛЕНИЕ 
            

            if (onDragEndInventory.maxCollection <= 1 && dragStartItemComponent.slot.amount > 1)
            {

                operation.AddItemInInventory(onDragEndInventory, dragStartItemComponent, int.Parse(_slot.name));
                dragStartItemComponent.slot.amount -= 1;
                if (dragStartItemComponent.slot.amount > 1)
                    dragStartItemComponent.text.text = dragStartItemComponent.slot.amount.ToString();
                else
                {
                    dragStartItemComponent.text.text = "";
                }

                operation.ItemCreate(_slot, onDragEndInventory, dragStartItemComponent);

            }
            //  ПЕРЕМЕЩЕНИЕ
            else
            {
                operation.AddItemInInventory(onDragEndInventory, dragStartItemComponent, int.Parse(_slot.name));
                operation.DeleteItem(dragStartItemComponent);
                operation.ItemRelocate(dragStartItemComponent, _slot, onDragEndInventory);
            }

            Debug.Log("NUM");
        }

        // перемещение в слот с итемом
        if (cellItem.Success)
        {
            ItemInInventory endDragItemComponent = _object[0].gameObject.GetComponent<ItemInInventory>();

            //КОЛЛЕКЦИОНИРОВАНИЕ

            if (dragStartItemComponent.slot.item.Name == endDragItemComponent.slot.item.Name)
                operation.CollectionItem(endDragItemComponent, dragStartItemComponent);
            else
            {
                //ВОЗМОЖНО ЗАМЕНА 
                operation.Exchange(dragStartItemComponent, endDragItemComponent);
            }

            Debug.Log("ITEM");
        }

        //перемещение в слот с итемом(мышь на количеством итема)
        if (cellText.Success)
        {
            ItemInInventory endDragItemComponent = _object[1].gameObject.GetComponent<ItemInInventory>();

            //КОЛЛЕКЦИОНИРОВАНИЕ

            if (dragStartItemComponent.slot.item.Name == endDragItemComponent.slot.item.Name)
                operation.CollectionItem(endDragItemComponent, dragStartItemComponent);
            else
            {
                //ВОЗМОЖНО ЗАМЕНА 
                operation.Exchange(dragStartItemComponent, endDragItemComponent);
            }

            Debug.Log("TEXT");
        }
    }
    else
    {
        operation.DropItem();
    }

    
    //TODO: TEMP
    if (_object.Count == 1)
    {
        if (SceneManager.GetActiveScene().name == "Hall")
        {
            HallSceneController.Instance.StopFollow();
        }
    }
}


/// <summary>
/// проверка на нахождение над объектом
/// </summary>
/// <returns></returns>
public List<RaycastResult> IsPointerOverUIObject() // проверка на нахождение над объектом
{
    PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
#if UNITY_STANDALONE || UNITY_EDITOR
    eventDataCurrentPosition.position =
        new Vector2(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y);
#else
    eventDataCurrentPosition.position = new Vector2(Touchscreen.current.position.ReadValue().x,
        Touchscreen.current.position.ReadValue().y);
#endif    
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
    return results;
}

}
