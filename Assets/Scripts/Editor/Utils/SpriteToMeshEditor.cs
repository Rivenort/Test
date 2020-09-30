using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Microsoft.SqlServer.Server;

/// <summary>
/// @author Dominik
/// </summary>
public class SpriteToMeshEditor :EditorWindow
{

    public static readonly Vector2 SIZE = new Vector2(300, 100);
    public static readonly string OUT_PATH = "Assets/Scripts/Editor/Out/";
    

    private Sprite m_sprite;

    [MenuItem(EditorPaths.MENU_UTILS + "TileSpriteToMesh")]
    public static void OpenWindow()
    {
        SpriteToMeshEditor window = GetWindow<SpriteToMeshEditor>();
        window.maxSize = SIZE;
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();

        m_sprite = (Sprite) EditorGUILayout.ObjectField(m_sprite, typeof(Sprite));
        if (GUILayout.Button("run")) {
            Mesh mesh = ConvSpriteToMesh(m_sprite);
            AssetDatabase.CreateAsset(mesh, OUT_PATH + m_sprite.name + "Mesh.asset");
            AssetDatabase.SaveAssets();
            EditorUtility.DisplayDialog("Sprite to Mesh conversion", "Mesh has been written into" + EditorPaths.PATH_OUTPUT_FILES + "", "ok");
        }

        GUILayout.EndHorizontal();
    }

    Vector2 maxN;
    Vector2 maxS;
    Vector2 maxW;
    Vector2 maxE;
    Mesh ConvSpriteToMesh(Sprite sprite)
    {
        Mesh mesh = new Mesh();
        /*
        mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
        mesh.uv = sprite.uv;
        mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);
        */
        maxN = sprite.vertices[0];
        maxS = sprite.vertices[0];
        maxE = sprite.vertices[0];
        maxW = sprite.vertices[0];
        for (int i = 1; i < sprite.vertices.Length; i++)
        {
            Vector2 temp = sprite.vertices[i];
            if (temp.y > maxN.y)
                maxN = temp;
            if (temp.y < maxS.y)
                maxS = temp;
            if (temp.x > maxE.x)
                maxE = temp;
            if (temp.x < maxW.x)
                maxW = temp;
        }
        Vector3[] vertices = new Vector3[4];
        maxN.x = 0;
        maxS.x = 0;
        maxE.y = 0;
        maxW.y = 0;
        vertices[0] = maxN;
        vertices[1] = maxE;
        vertices[2] = maxS;
        vertices[3] = maxW;
        Debug.Log("maxN: " + maxN);
        Debug.Log("maxE: " + maxE);
        Debug.Log("maxS: " + maxS);
        Debug.Log("maxW: " + maxW);

        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0.5f, 1);
        uv[1] = new Vector2(1, 0.5f);
        uv[2] = new Vector2(0.5f, 0f);
        uv[3] = new Vector2(0f, 0.5f);

        int[] triangles = new int[2 * 3];
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] = 1;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        


        return mesh;
    }
}
