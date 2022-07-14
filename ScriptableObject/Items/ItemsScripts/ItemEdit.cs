
/*
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class ItemEdit : UnityEditor.Editor
{
    protected ItemObject item = null;
    private List<string> components = new List<string>();
    private bool flag = default;
    protected virtual void OnEnable() 
    {
        
        this.item = target as ItemObject;
        Debug.Log(item.name);
        
    }

    protected virtual void OnDisable()
    {
        EditorUtility.SetDirty(this.item);
    }

public override void OnInspectorGUI()
{
    if (!item) return;
    {
        if (flag != true)
        {
            EditorGUILayout.LabelField("ID: ", this.item.id.ToString());
            EditorGUILayout.LabelField("name: ", this.item.Name);
            EditorGUILayout.TextArea("Описание: ", this.item.description);
            Sprite ing = EditorGUILayout.ObjectField(item.uiDisplay, typeof(Sprite), false, GUILayout.Width(60), GUILayout.Height(60)) as Sprite;
            EditorGUILayout.LabelField("Стоимость: ", this.item.coast.ToString());
            EditorGUILayout.LabelField("Занимаемое место: ", this.item.cells.ToString());
            
            
            EditorGUILayout.HelpBox("You can add or remove components from 'Components' folder.", MessageType.Info);
            GUILayout.Space(10);
        }
        else
        {
            EditorGUILayout.HelpBox("YEAH.", MessageType.Info);
            GUILayout.Space(10);
        }

        if (GUILayout.Button("Edit"))
        {
            flag = true;
        }

    }

    
}



private void DrawComponents() {
    foreach(string component in this.components) {
        Rect r = GUILayoutUtility.GetRect(100, 20);
        GUI.Box(r, component);
        GUILayout.Space(5);
    }
}


		
}
    
[CustomEditor(typeof(ItemObject))]
public sealed class ItemObjectEditor : ItemEdit {
    protected override void OnEnable() {
        base.OnEnable();
    }
  
    protected override void OnDisable() {
        base.OnDisable();
    }
		
    public override void OnInspectorGUI()	{
        base.OnInspectorGUI();
    }

}*/
