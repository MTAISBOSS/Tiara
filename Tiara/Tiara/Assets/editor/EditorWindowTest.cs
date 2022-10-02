using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorWindowTest : EditorWindow
{
    Color _color = Color.yellow;

    private Texture2D header;
    private Rect area;
    
    [MenuItem("Window/Editor Test")]
    static void OpenWindow()
    {
        EditorWindowTest windowTest = (EditorWindowTest)GetWindow(typeof(EditorWindowTest));
        windowTest.minSize = new Vector2(600, 300);
        windowTest.Show();
    }

    private void OnEnable()
    {
        InitTexture();
    }

    void InitTexture()
    {
        header = new Texture2D(1, 1);
        header.SetPixel(0,0,_color);
        header.Apply();
    }
    private void OnGUI()
    {
        DrawLayOuts();
    }

    private void DrawLayOuts()
    {
        area.x = 0;
        area.y = 0;
        area.width = Screen.width;
        area.height = 50;
        
        GUI.DrawTexture(area,header);


    }
}