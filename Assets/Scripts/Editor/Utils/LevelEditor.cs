using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class LevelEditor : EditorWindow
{
    public static readonly Vector2 SIZE = new Vector2(400, 300);
    private const float AREA_HEIGHT = 30f;

    private static DAT_Game s_data;
    private static int s_mapW, s_mapH;
    private static GameObject s_tilePrefab;

    [MenuItem(itemName: EditorPaths.MENU_UTILS + "LevelEditor ")]
    private static void OpenWindow()
    {
        LevelEditor window = GetWindow<LevelEditor>();
        window.maxSize = SIZE;
        window.minSize = SIZE;
        window.Show();
    }

    private void OnEnable()
    {
        if (s_data == null)
            s_data = CreateInstance<DAT_Game>();
    }

    private void OnGUI()
    {
        Rect area = new Rect(0, 0, Screen.width, AREA_HEIGHT);
        GUILayout.BeginArea(area);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Level: ");
        //s_data.prefabLevel = (GameObject) EditorGUILayout.ObjectField(s_data.prefabLevel, typeof(GameObject));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        area.y += AREA_HEIGHT;

        GUILayout.BeginArea(area);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Resources: ");
        s_data.resources = (DAT_Resources)EditorGUILayout.ObjectField(s_data.resources, typeof(DAT_Resources));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        area.y += AREA_HEIGHT;

        GUILayout.BeginArea(area);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Map size: ");
        s_mapW = EditorGUILayout.IntField(s_mapW);
        s_mapH = EditorGUILayout.IntField(s_mapH);

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        area.y += AREA_HEIGHT;

        GUILayout.BeginArea(area);
        GUILayout.BeginHorizontal();

        GUILayout.Label("Tile prefab: ");
        s_tilePrefab = (GameObject)EditorGUILayout.ObjectField(s_tilePrefab, typeof(GameObject));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        area.y += AREA_HEIGHT;

        GUILayout.BeginArea(area);
        
        if (GUILayout.Button("Generate") && s_tilePrefab != null)
        {
            //s_data.tiledMap = new DAT_TiledMap(s_mapW, s_mapH, s_tilePrefab, true);

            AssetDatabase.CreateAsset(s_data, EditorPaths.PATH_OUTPUT_FILES + "newLevelData.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.DisplayDialog("LevelEditor", "Level has been written into" + EditorPaths.PATH_OUTPUT_FILES + ".", "ok");
        }

        GUILayout.EndArea();
    }
}
