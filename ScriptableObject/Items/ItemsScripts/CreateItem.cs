using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;


public static class EditorSettings
{

    public static Vector2 WindowSize = new Vector2(300, 800);

    [MenuItem("Window/Inventory/Item")]
    public static void InitWindow()
    {
        ItemCreate window = (ItemCreate) EditorWindow.GetWindow(typeof(ItemCreate));
        window.maxSize = window.minSize = WindowSize;
        if (ItemCreate.defStyle == null)
        {
            ItemCreate.defStyle = new GUIStyle();
            ItemCreate.defStyle.alignment = TextAnchor.LowerCenter;
            ItemCreate.defStyle.fontStyle = FontStyle.Bold;
        }
    }
}


public class ItemCreate : EditorWindow
{
    public static GUIStyle defStyle = null;
    public static ItemObject _item = ItemObject.CreateInstance<ItemObject>();
    private static ItemDatabaseObject _db = ItemDatabaseObject.CreateInstance<ItemDatabaseObject>();
    string _name = default, _description = default, _fileName = default;
    Sprite _uiDisplay, _dragDisplay = default;
    int _cell = default;
    float _coast = default;
    
    
    
    private void OnGUI()
    {
        if (this.isEditor)
        {
            UpdateWindowGUI();
        }
    }

    private void UpdateWindowGUI()
    {
        DrawMainStartGUI();
        GUILayout.Space(10);
    }

    private void DrawMainStartGUI()
    {
        Rect r = GUILayoutUtility.GetRect(EditorSettings.WindowSize.x, 15);
        r.height += 10;
        GUI.Box(r, "Новый предмет");
        GUILayout.Space(10);
        _db = EditorGUILayout.ObjectField(_db, typeof(ItemDatabaseObject), false,GUILayout.Width(200), GUILayout.Height(20)) as ItemDatabaseObject;
        GUILayout.Space(10);
        _fileName = EditorGUILayout.TextField("Имя файла", _fileName);
        GUILayout.Label("Тип предмета");
        _item.type = (ItemType) EditorGUILayout.EnumPopup(_item.type);
        GUILayout.Space(10);
        _name = EditorGUILayout.TextField("Название: ", _name);
        GUILayout.Space(10);
        GUILayout.Label("Иконка в инвентаре");
        _uiDisplay = EditorGUILayout.ObjectField(_item.uiDisplay, typeof(Sprite), false, GUILayout.Width(60), GUILayout.Height(60)) as Sprite;
        GUILayout.Label("Иконка при перетаскивании предмета");
        _dragDisplay = EditorGUILayout.ObjectField(_item.dragIcon, typeof(Sprite), false, GUILayout.Width(60), GUILayout.Height(60)) as Sprite;
        GUILayout.Space(10);
        GUILayout.Label("Описание");
         _description = EditorGUILayout.TextArea(_description, GUILayout.Height(100));
         GUILayout.Space(10);
         GUILayout.Label("Стоимость");
         _coast = EditorGUILayout.FloatField(_coast);
         GUILayout.Space(10);
         GUILayout.Label("Занимаемые ячейки");
         _cell = EditorGUILayout.IntField(_cell);
         GUILayout.Space(10);
         switch (_item.type)
         {
             case ItemType.Potion:
             {
  
                 GUILayout.Space(10);
                 break;
             }
             case ItemType.Scroll:
             {
                 GUILayout.Space(10);
                 break;
             }
             case ItemType.Weapon:
             {
                 GUILayout.Space(10);
                 break;
             }

             case ItemType.Armor:
             {
                 GUILayout.Space(10);
                 break;
             }

             case ItemType.Money:
             {
                 GUILayout.Space(10);
                 GUILayout.Label("НЕСКОЛЬКО ПРЕДМЕТОВ В ОДНОЙ ЯЧЕЙКЕ");
                 GUILayout.Space(10);
                // _item.stackable = true;
                 break;
             }
         }
         
         GUILayout.Space(5);
        if (GUILayout.Button("Create"))
        {
            if (_fileName != null && _fileName != "")
            {
                _item = new ItemObject(); 
                _item.ID = Guid.NewGuid();
                string path = "Assets/Asset/Inventory/Items/ItemsAsset/" + _fileName + ".asset";
                _item.Name = _name;
                _item.description = _description;
                _item.cells = _cell;
                _item.coast = _coast;
                _item.uiDisplay = _uiDisplay;
                _item.dragIcon = _dragDisplay;
                AssetDatabase.CreateAsset(_item, path);
                _db.BaseItem.Add(_item);
                AssetDatabase.SaveAssets();
                this.Close();
            }
            else
            {
                EditorGUILayout.HelpBox("А не заполнить ли нам поля???.", MessageType.Info);
            }
            
        }
    }

    public bool isEditor
    {
        get { return Application.isPlaying == false; }
    }
}
#endif