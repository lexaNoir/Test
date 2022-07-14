using System;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;


public static class EditorBase
{

    public static Vector2 WindowSize = new Vector2(150, 150);

    [MenuItem("Window/Inventory/Database")]
    public static void InitWindow()
    {
        DatabaseCreate window = (DatabaseCreate) EditorWindow.GetWindow(typeof(DatabaseCreate));
        window.maxSize = window.minSize = WindowSize;
        if (DatabaseCreate.defStyle == null)
        {
            DatabaseCreate.defStyle = new GUIStyle();
            DatabaseCreate.defStyle.alignment = TextAnchor.LowerCenter;
            DatabaseCreate.defStyle.fontStyle = FontStyle.Bold;
        }
    }
}


public class DatabaseCreate : EditorWindow
{
    public static GUIStyle defStyle = null;
    private static ItemDatabaseObject _db = CreateInstance<ItemDatabaseObject>();

    private void OnGUI()
    {
        if (this.isEditor)
        {
          
            GUILayout.Space(15);
            if (GUILayout.Button("Create"))
            {
                string path = "Assets/Asset/Inventory/Database/DatabaseAsset/database.asset";
                AssetDatabase.CreateAsset(_db, path);
                AssetDatabase.SaveAssets();
                this.Close();
                
            }
        }
    }

    public bool isEditor
    {
        get { return Application.isPlaying == false; }
    }
}
#endif

public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    
    public List<ItemObject> BaseItem;

    public void UpdateId()
    {
        for (int i = 0; i < BaseItem.Count; i++)
        {
            if(BaseItem[i] != null)
            {
                if (BaseItem[i].ID == Guid.Empty)
                {
                    BaseItem[i].ID = Guid.NewGuid();
                    BaseItem[i].ItemId = BaseItem[i].ID.ToString();
                }
            }
        }
    }

    public void OnAfterDeserialize()
    {
        UpdateId();
    }

    public void OnBeforeSerialize()
    {
    }
}