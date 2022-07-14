using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class InventoryFunction : InventoryLogic
{
    private List<ItemObject> itemDatabase;
    private Inventory playerInventory;
    private Item _item;
    private GameObject tempItem;
    private float speed;
    [Inject] private Player player;
    

    // Сюда нужна  dstsbase и inventory
    [Inject]
    public void Construct(Inventory inventory, ItemDatabaseObject itemDatabaseObject)
    {
        itemDatabase = itemDatabaseObject.BaseItem;
        playerInventory = inventory;
    }

    //TODO: TEST 
    #region Test
    
    public void Start()
    {
        testFunctionAddItemOnInventory("Золото", 5);
        testFunctionAddItemOnInventory("Potion");
        testFunctionAddItemOnInventory("LightBolt");
    }
    
    
    //Чистой воды чит для добавления конкретного предмета
    public void testFunctionAddItemOnInventory(string itemName, int _amount = default)
    {
        for (int i = 0; i < playerInventory.Slot.Length; i++)
        {
            if (playerInventory.Slot[i].ItemCellId == -1)
            {
                playerInventory.Slot[i].item = AddItem(itemName);
                playerInventory.Slot[i].ItemCellId = i;
                if (_amount > 0)
                    playerInventory.Slot[i].amount += _amount;
                else
                    playerInventory.Slot[i].amount += 1;
                return;
            }
        }
    }
    

    public Item AddItem(string id_or_name)
    {

        if (id_or_name != null)
        {
            foreach (ItemObject item in itemDatabase)
            {
                if (item.Name == id_or_name || item.name == id_or_name)
                {
                    _item = item.CreateItem();
                    return _item;
                }
            }
        }

        //return null;
        return new Item();
    }
    
    #endregion

    
    /// <summary>
    /// Создание итема при разделении
    /// </summary>
    /// <param name="_itemInInventory"> откуда тянем(если default, то создается итем при старте инвентаря)</param>
    /// <param name="_slot"> ячейка инвентаря в которую тянем</param>
    /// <param name="onDragEndInventory"> Инвентарь в который тянем  </param>
    public void ItemCreate(GameObject objSlot, Inventory onDragEndInventory, ItemInInventory _itemInInventory = default)
    {
        var slot = int.Parse(objSlot.name);
        Vector2 newPosition = onDragEndInventory.Slot[slot].anhoredPosition;

        GameObject _Item = Instantiate(Resources.Load<GameObject>("itemInInventory"), transform, false);
        ItemInInventory ItemComponents = _Item.GetComponent<ItemInInventory>();

        _Item.transform.SetParent(onDragEndInventory.Slot[slot].transform);
        ItemComponents.Inventory = onDragEndInventory;
        ItemComponents.slot = onDragEndInventory.Slot[slot];
        if (onDragEndInventory.Slot[slot].amount >= 2)
            ItemComponents.text.text = onDragEndInventory.Slot[slot].amount.ToString();
        else
            ItemComponents.text.text = "";
        ItemComponents.RTransform.anchoredPosition = newPosition;

        if (_itemInInventory == default)
        {
            ItemComponents.image.sprite = onDragEndInventory.Slot[slot].item.UIDisplay;
        }
        else
        {
            ItemComponents.image.sprite = _itemInInventory.slot.item.UIDisplay;

            /*if (_itemInInventory.slot.amount >= 2)
                ItemComponents.text.text = _itemInInventory.slot.amount.ToString();*/
        }

        EventAction(_Item, EventTriggerType.BeginDrag, delegate { DragStart(ItemComponents); });
        EventAction(_Item, EventTriggerType.Drag, delegate { Drag(); });
        EventAction(_Item, EventTriggerType.EndDrag, delegate { DragEnd(ItemComponents); });
    }


    public void EventAction(GameObject obj, EventTriggerType triggerType, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = triggerType;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }


    /// <summary>
    ///  Начало перемещение объекта
    /// </summary>
    /// <param name="itemComponent"></param>
    void DragStart(ItemInInventory itemComponent)
    {
        tempItem = new GameObject();
        RectTransform rt = tempItem.AddComponent<RectTransform>();
        rt.sizeDelta = itemComponent.RTransform.sizeDelta;
        tempItem.transform.SetParent(transform);
        Image img = tempItem.AddComponent<Image>();
        rt.transform.localScale = new Vector3(1, 1, 1);
        img.sprite = itemComponent.slot.item.DragDisplayItem;
        img.raycastTarget = false;
        tempItem.name = itemComponent.slot.item.Name;
        ItemInInventory itemSlot = tempItem.AddComponent<ItemInInventory>();
    }

    
    void Drag()
    {
        List<RaycastResult> _object = IsPointerOverUIObject();
        Vector2 pos;
        
#if UNITY_STANDALONE || UNITY_EDITOR
        tempItem.transform.position = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
#else
        tempItem.transform.position = Touchscreen.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(Touchscreen.current.position.ReadValue());
        
#endif   
        
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mousePos = hit.point;
            Vector3 fromTo = mousePos - player.transform.position;
            Vector3 XZ = new Vector3(fromTo.x, 0, fromTo.z);
            
            player.transform.rotation = Quaternion.LookRotation(XZ, Vector3.up);
            
            float x = XZ.magnitude;
            float y = fromTo.y;

            float radians = 45 * Mathf.PI / 180;

            float v2 = (Physics.gravity.y * x * x) /
                       (2 * (y - Mathf.Tan(radians) * x) * Mathf.Pow(Mathf.Cos(radians), 2));
            speed = Mathf.Sqrt(Mathf.Abs(v2));
            
            if (_object.Count <= 1)
                player.Thrownlenerender.ShowLineItem(player.transform.position,
                    player.Throwtransform.forward * speed);
            else
                player.Thrownlenerender.ShowLineItem(Vector3.zero, Vector3.zero);
        }
        
        IsPointerOverUIObject();
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemComponent"></param>
    void DragEnd(ItemInInventory itemComponent)
    {
        OnDragEnd(itemComponent);
        Destroy(tempItem);
    }

    
    public void ItemRelocate(ItemInInventory _itemInInventory, GameObject _slot, Inventory onDragEndInventory)
    {
        int slot = int.Parse(_slot.name);
        Vector2 newPosition = onDragEndInventory.Slot[slot].anhoredPosition;
        _itemInInventory.gameObject.transform.SetParent(_slot.transform.parent);
        _itemInInventory.RTransform.anchoredPosition = newPosition;
        _itemInInventory.slot = onDragEndInventory.Slot[slot];
        _itemInInventory.Inventory = onDragEndInventory;
        Destroy(tempItem);
    }

    public void Exchange(ItemInInventory _dragStartItemComponent, ItemInInventory _dragEndItemComponent)
    {
        if (_dragEndItemComponent.Inventory.maxCollection < _dragStartItemComponent.slot.amount ||
            _dragStartItemComponent.Inventory.maxCollection < _dragEndItemComponent.slot.amount)
            return;

        Item tempInventoryDragEndItem = _dragStartItemComponent.slot.item;
        string dragStartItemCount = _dragStartItemComponent.text.text;

        // замена местами предметов в инвентаре
        _dragStartItemComponent.slot.item = _dragEndItemComponent.slot.item;
        _dragEndItemComponent.slot.item = tempInventoryDragEndItem;

        //изменение информации о items в физическом предмете
        _dragStartItemComponent.image.sprite = _dragStartItemComponent.slot.item.UIDisplay;
        _dragEndItemComponent.image.sprite = tempInventoryDragEndItem.UIDisplay;

        // замена количесва местами
        _dragStartItemComponent.text.text = _dragEndItemComponent.text.text;
        _dragEndItemComponent.text.text = dragStartItemCount;
        if (_dragStartItemComponent.text.text != "")
            _dragStartItemComponent.slot.amount = int.Parse(_dragStartItemComponent.text.text);
        else
            _dragStartItemComponent.slot.amount = 1;

        if (_dragEndItemComponent.text.text != "")
            _dragEndItemComponent.slot.amount = int.Parse(_dragEndItemComponent.text.text);
        else
            _dragEndItemComponent.slot.amount = 1;
        
        Destroy(tempItem);
    }

    public void AddItemInInventory(Inventory _onDragEndInventory, ItemInInventory _dragStartItemComponent, int numSlot)
    {
        //добавление нового итема в инвентарь

        //ДОБАВИТЬ УСЛОВИЕ ПРИ КОТОРОМ ПРОВЕРЯЕТСЯ ТИП ПРЕДМЕТОВ, КОТОРЫЕ МОГУТ БЫТЬ В ИНВЕНТАРЕ

        _onDragEndInventory.Slot[numSlot].item = _dragStartItemComponent.slot.item;
        _onDragEndInventory.Slot[numSlot].ItemCellId = numSlot;

        if (_onDragEndInventory.maxCollection > 1)
            _onDragEndInventory.Slot[numSlot].amount += _dragStartItemComponent.slot.amount;
        else
            _onDragEndInventory.Slot[numSlot].amount += 1;
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="startDragItemComponent"> Данные ячейки из которй тянем </param>
    /// <param name="_obj"> удаление физического объекта (startDragItemComponent.gameObject) </param>
    public void DeleteItem(ItemInInventory startDragItemComponent, GameObject _obj = default)
    {
        //удаление старого итема
        startDragItemComponent.slot.amount = 0;
        startDragItemComponent.slot.item = new Item();
        startDragItemComponent.slot.ItemCellId = -1;
        if (_obj)
        {
            Destroy(_obj);
        }
    }

    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="endDragItemComponent"> Данные ячейки В КОТОРЫЙ перетягиваем </param>
    /// <param name="startDragItemComponent"> Данные ячейки из КОТОРЫЙ </param>
    public void CollectionItem(ItemInInventory endDragItemComponent, ItemInInventory startDragItemComponent)
    {
        if (endDragItemComponent.Inventory.maxCollection == 1 ||
            (endDragItemComponent.slot.amount + startDragItemComponent.slot.amount) >
            endDragItemComponent.Inventory.maxCollection) return;

        if (endDragItemComponent.slot.item.Type == "Money")
        {
            endDragItemComponent.slot.amount += startDragItemComponent.slot.amount;
            endDragItemComponent.text.text = endDragItemComponent.slot.amount.ToString();
            DeleteItem(startDragItemComponent, startDragItemComponent.gameObject);
            Destroy(tempItem);
        }
    }

    
    public void DropItem()
    {
        GameObject newBullet = Instantiate(Bullet, player.Throwtransform.position,
            quaternion.identity);
        newBullet.GetComponent<Rigidbody>().velocity = player.Throwtransform.forward * (speed - 1);
        player.Thrownlenerender.ShowLineItem(Vector3.zero, Vector3.zero);

        //TODO: Test
#if DEMO_VIDEO
            if (SceneManager.GetActiveScene().name == "Guardpost")
            {
                GuardControllerGuardpost.instance.Pig();
            }
#endif
    }
}
