using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using Boo.Lang.Runtime;

/// <summary>
/// The following class parses a XML file with buildings data into a scriptable object data.
/// </summary>
public class XMLBuildingsLoader : EditorWindow
{
    private static string filename = "Game/Buildings/buildings";
    private static string outputDir = "Game/Buildings/";
    public static readonly Vector2 SIZE = new Vector2(400, 200);

    


    [MenuItem(EditorPaths.MENU_UTILS + "XMLBuildingsLoader")]
    private static void OpenWindow()
    {
        XMLBuildingsLoader window = GetWindow<XMLBuildingsLoader>();
        window.minSize = SIZE;
        window.maxSize = SIZE;
    }


    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        GUILayout.Label("Filename: ");
        filename = GUILayout.TextField(filename);

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.Label("Output: ");
        outputDir = GUILayout.TextField(outputDir);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Parse"))
        {
            parseBuildingSheet(filename, outputDir);
        }

        GUILayout.EndHorizontal();

        GUILayout.EndHorizontal();
    }

    public void parseBuildingSheet(string filename, string output)
    {

        DAT_BuildingTemplates data = XMLBuildingsParser.Parse(filename);



        EditorUtility.SetDirty(data);
        AssetDatabase.CreateAsset(data, "Assets/Resources/" + output + "buildings.asset");
        AssetDatabase.SaveAssets();
    }



}
