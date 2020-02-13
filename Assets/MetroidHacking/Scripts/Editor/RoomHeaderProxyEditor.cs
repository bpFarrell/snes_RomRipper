using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(RoomHeaderProxy))]
public class RoomHeaderProxyEditor : Editor
{
    private void OnEnable(){

    }
    public override void OnInspectorGUI()
    {
        RoomHeaderProxy room = (RoomHeaderProxy)target;
        EditorGUILayout.LabelField(((int)room.header.roomArea)+": " +room.header.roomArea.ToString());
        EditorGUILayout.Separator();

        EditorGUILayout.LabelField("Positional data");
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.TextField("Index" , room.header.roomIndex.ToString("X2"));
            EditorGUILayout.TextField("Offset" , room.header.headerOffset.ToString("X6"));
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Pointers", new []{GUILayout.Width(130)});
        EditorGUILayout.Vector2Field("Translation x,y", room.header.pos);
        EditorGUILayout.LabelField("Pointers", new []{GUILayout.Width(130)});
        EditorGUILayout.Vector2Field("Size w,h", room.header.size);

        EditorGUILayout.Separator();
        EditorGUILayout.LabelField("Pointers", new []{GUILayout.Width(130)});

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("ptrDoorOut", new []{GUILayout.Width(130)});
            GUIPtr(room.header.ptrDoorOut);
            EditorGUILayout.LabelField("ptrLevelData", new []{GUILayout.Width(130)});
            GUIPtr(room.header.ptrLevelData,"X6");
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("ptrFX1", new []{GUILayout.Width(130)});
            GUIPtr(room.header.ptrFX1);
            EditorGUILayout.LabelField("ptrFX2", new []{GUILayout.Width(130)});
            GUIPtr(room.header.ptrFX2);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("ptrEnemyPop", new []{GUILayout.Width(130)});
            GUIPtr(room.header.ptrEnemyPop);
            EditorGUILayout.LabelField("ptrEnemySet", new []{GUILayout.Width(130)});
            GUIPtr(room.header.ptrEnemySet);
        }
        EditorGUILayout.EndHorizontal();
        //EditorGUILayout.EndHorizontal();
        //EditorGUILayout.LabelField("Level",room.header.);
    }
    void GUIPtr(string title, uint ptr, string parse = "X4")
    {

        EditorGUILayout.TextField(title, ptr.ToString(parse));
        if(GUILayout.Button(" ")){
            HexView.InitAtOffset(ptr);
        }
    }
    void GUIPtr(uint ptr, string parse = "X4")
    {

        EditorGUILayout.TextField(ptr.ToString(parse));
        if(GUILayout.Button("")){
            HexView.InitAtOffset(ptr);
        }
    }
}
