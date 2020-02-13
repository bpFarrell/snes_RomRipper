using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public class HexView : EditorWindow{
    private static Font _font;

    private static Font font{
        get
        {
            if (_font == null){
                _font = Resources.Load("cour") as Font;
            }
            return _font;
        }
    }
    Rect rect {
        get { return this.position; }
    }
    int scrollPos = 0;
    int bytesPerCol = 32;
    int posOffset {
        get { return scrollPos * bytesPerCol; }
        set { scrollPos = Mathf.FloorToInt(value / bytesPerCol); }
    }
    int lineCount {
        get { return (int)((rect.height - 32) / 18f); }
    }
    int totalBytesInView {
        get { return bytesPerCol * lineCount; }
    }
    [MenuItem("Tools/Hex Editor")]
    static HexView Init()
    {
        HexView window = (HexView)EditorWindow.GetWindow(typeof(HexView));
        window.maxSize = new Vector2(1100f, 1000f);
        window.minSize = new Vector2(1100f, 100f); 
        window.Show();
        return window;
    }
    public static void InitAtOffset(uint offset)
    {
        HexView window = Init();
        window.GotoOffset(offset);
    }
    void OnGUI()
    {
        Debug.Log(rect.height);

        GUI.skin.font = font;
        GUILayout.BeginHorizontal();
        GUILayout.Label("Hex View");
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();

        string offsetField = "";
        for(int x = scrollPos; x < lineCount+scrollPos; x++)
        {
            offsetField += "0x"+(x*bytesPerCol).ToString("X6")+"\n";
        }
        GUILayout.TextField(offsetField, GUILayout.Width(90));
        StringBuilder valueField = new StringBuilder();
        //string valueField = "";
        byte[] block = Rom.smRom.GetDataBlock(posOffset, totalBytesInView);
        for(int x = 0; x < block.Length; x++)
        {
            valueField.Append(block[x].ToString("X2"));
            if (x % bytesPerCol == bytesPerCol - 1)
                valueField.Append("\n");
            else
                valueField.Append(" ");
        }

        GUILayout.TextField(valueField.ToString(), GUILayout.Width(965));


        GUILayout.BeginVertical();
        if (GUILayout.Button("^", GUILayout.Height(32)))
        {
            scrollPos -= 3;
            scrollPos = Mathf.Max(scrollPos, 0);
        }
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("V", GUILayout.Height(32)))
        {
            scrollPos += 3;
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    public void GotoOffset(uint offset)
    {
        posOffset = (int)offset;
    }
}
