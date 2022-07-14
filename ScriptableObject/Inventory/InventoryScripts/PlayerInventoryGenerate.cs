using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class PlayerInventoryGenerate : MonoBehaviour
    {
        //создание инвентаря и его регистрация

      //  public Dictionary<GameObject, InventorySlot> InventoryItem;
        //public List<InventorySlot> InventoryItem;
        /*[HideInInspector]*/ public List<GameObject> InventoryItem;
        public Inventory _inventory;
        public int X_START;
        public int Y_START;
        public float X_SPACE_BETWEEN_ITEM;
        public int NUMBER_OF_COLUMN;
        public float Y_SPACE_BETWEEN_ITEMS;
        
       // public static event Action<Inventory, Dictionary<GameObject, InventorySlot>> InventoryOpen;
       // public static event Action<Inventory, List<InventorySlot>> InventoryOpen;
        public static event Action<Inventory, List<GameObject>> InventoryOpen;


        private Vector3 GetPosition(int i)
        {
            return new Vector2(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)),
                Y_START + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMN)));
        }

        public void InventoryCreate()
        {
           //InventoryItem = new List<InventorySlot>();
           InventoryItem = new List<GameObject>();
            for (int i = 0; i < _inventory.Slot.Length; i++)
            {
                GameObject obj = Instantiate(Resources.Load<GameObject>("inventoryCell"), Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(0f, 0f, 0f);
                obj.GetComponent<RectTransform>().anchoredPosition = GetPosition(i);
                _inventory.Slot[i].ID = i;
                _inventory.Slot[i].anhoredPosition = GetPosition(i);
                _inventory.Slot[i].transform = obj.transform.parent;
           //     _inventory.Slot[i].beInventory = _inventory;
                obj.name = i.ToString();
                
                //InventoryItem.Add(_inventory.Slot[i]);
                InventoryItem.Add(obj);
                
            }
         }

        private void Start()
        {
            if(InventoryItem.Count == 0)
              OnEnableInventory();
        }

        public void OnEnableInventory(GameObject _obj = default)
        {
            if (_obj)
            {
                for (int i = _obj.transform.childCount; i > 0; --i)
                {
                    DestroyImmediate(_obj.transform.GetChild(0).gameObject);
                }
            }
            InventoryCreate();
            InventoryOpen?.Invoke(_inventory, InventoryItem);
        }


    }

