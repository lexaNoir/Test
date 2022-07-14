using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Zenject;

public class Open_Close_Inventory : MonoBehaviour
{
    public GameObject _quickPanel, _inventoryActive, _inventoryPanel, _beltPanel, _miniMap;
    public PlayerInventoryGenerate _inventoryGenerate;
    private SignalBus signalBus;
    private bool isOpen = false;
    
    [Inject]
    public void Construct(SignalBus signalBus)
    {
        this.signalBus = signalBus;
    }

    private void OnEnable()
    {
        signalBus.Subscribe<ButtonClick>(OnInventoryClick);
    }

    private void OnDisable()
    {
        signalBus.TryUnsubscribe<ButtonClick>(OnInventoryClick);
    }

    void OnInventoryClick(ButtonClick buttonClick)
    {
        if (buttonClick.ButtonType == ButtonsType.Inventory)
        {
            if (isOpen)
            {
                PlayerInventoryClose();
            }

            else
            {
                PlayerInventoryOpen();
            }
        }
    }


    public void PlayerInventoryOpen()
    {
        _quickPanel.SetActive(false);
        _inventoryActive.SetActive(true);
        _inventoryGenerate.OnEnableInventory(_inventoryPanel);
     //   _inventoryGenerate.OnEnableInventory(_beltPanel);
    }

    public void PlayerInventoryClose()
    {
        signalBus.Fire(new ButtonClick{ButtonType = ButtonsType.Inventory});
        _quickPanel.SetActive(true);
        _inventoryActive.SetActive(false);
    }
}
