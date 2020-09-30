using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

/// <summary>
/// @author Dominik 
/// </summary>
public class MapGeneratorEditor : EditorWindow
{

    public static readonly Vector2 SIZE = new Vector2(200, 100);

    private static readonly Rect AREA1 = new Rect(0, 0, SIZE.x, 40);
    private static readonly Rect AREA2 = new Rect(0, 40, SIZE.x, 40);
    private static readonly Rect AREA3 = new Rect(0, 80, SIZE.x, 40);

    private int m_mapW = 10;
    private int m_mapH = 10;
    private float m_tileW = 1.7f;
    private float m_tileH = 2f;
    private static GameObject s_tilePrefab = null;
    private static GameObject s_tilePrefab2 = null;

    private GameObject[] m_tiles;

 
    [MenuItem(itemName: EditorPaths.MENU_UTILS + "MapGenerator")]
    private static void OpenWindow()
    {
        MapGeneratorEditor window = GetWindow<MapGeneratorEditor>();
        window.maxSize = SIZE;
        window.minSize = SIZE;
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(AREA1);
        GUILayout.BeginHorizontal();

        GUILayout.Label("width: ", GUILayout.Width(50));
        m_mapW = EditorGUILayout.IntField(m_mapW, GUILayout.Width(30));
        GUILayout.Label("height: ", GUILayout.Width(50));
        m_mapH = EditorGUILayout.IntField(m_mapH, GUILayout.Width(30));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUILayout.BeginArea(AREA2);
        GUILayout.BeginHorizontal();

        GUILayout.Label("prefab: ", GUILayout.Width(50));
        s_tilePrefab = (GameObject)EditorGUILayout.ObjectField(s_tilePrefab, typeof(GameObject));
        GUILayout.Label("prefab2: ", GUILayout.Width(50));
        s_tilePrefab2 = (GameObject)EditorGUILayout.ObjectField(s_tilePrefab2, typeof(GameObject));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        GUILayout.BeginArea(AREA3);

        if (GUILayout.Button("Generate"))
        {
            Generate();
        }

        GUILayout.EndArea();
    }

    private void Generate()
    {
        GameObject groupLevel = M_GameHelper.SGetGroup(M_GameHelper.Group.LEVEL);
        if (groupLevel == null)
        {
            Debug.LogWarning("Level group does not exist! Make sure that a Level gameobject is in the Hierarchy.");
            return;
        } else
        {
            // Delete current terrain.
            GameObject groupTerrain = M_GameHelper.SGetGroup(M_GameHelper.Group.TERRAIN);
            if (groupTerrain == null)
            {
                Debug.LogWarning("Terrain group does not exist! Make sure that a Terrain gameobject is in the Hierarchy.");
                return;
            }

            int count = groupTerrain.transform.childCount;
            for (int i = 0; i < count; i++)
            {
                M_GameHelper.DestroyImmediate(groupTerrain.transform.GetChild(0).gameObject);
            }
        }


        m_tiles = new GameObject[m_mapW * m_mapH];

        for (int i = 0; i < m_mapW; i++)
        {
            for (int j = 0; j < m_mapH; j++)
            {
                GameObject obj = null;

                // choose a sprite for a tile
                if (i % 2 == 1)
                {
                    obj = (GameObject)PrefabUtility.InstantiatePrefab(s_tilePrefab);
                } else
                {
                    obj = (GameObject)PrefabUtility.InstantiatePrefab(s_tilePrefab2);
                }
                // TileData setup
                obj.GetComponent<Tile>().i = i;
                obj.GetComponent<Tile>().j = j;
                obj.GetComponent<Tile>().data = new DAT_Tile(false);
                
                // Position and name setup
                Vector3 temp = new Vector3(i * m_tileW, 0, j * m_tileH);
                if (i % 2 == 1)
                {
                    temp.z -= m_tileH/2;
                }
                
                obj.transform.position = temp;
                obj.name = "obj" + i + "_" + j;

                // Apply to structures
                M_GameHelper.AddToGroup(obj, M_GameHelper.Group.TERRAIN);
                m_tiles[j * m_mapW + i] = obj;
            }
        }



        if (groupLevel != null)
        {
            groupLevel.GetComponent<Level>().tiledMap = new DAT_TiledMap(m_mapW, m_mapH, m_tiles);
            EditorUtility.SetDirty(groupLevel.GetComponent<Level>());
        }
    }
}
