using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(EnvironmentElement))]
public class EnvironmentElementEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnvironmentElement element = (EnvironmentElement)target;


        if (GUILayout.Button("ApplyToTilemap", GUILayout.Width(200)))
        {
            if (element.tile != null)
            {
                element.tile.SetOccupied(false);
                EditorUtility.SetDirty(element.tile);
            }
                

            Vector3 pos = element.transform.position;
            pos.y -= 0.1f;
            GameObject collider = M_GameHelper.SGetObjectAtWorldPoint(pos);

            Tile tile = collider.transform.parent.gameObject.GetComponent<Tile>();

            if (tile != null)
            {
                pos = tile.gameObject.transform.position;
                pos.y = M_MapManager.SGetYDepthValue(pos.z);
                element.transform.position = pos;
                element.tile = tile;
                tile.SetOccupied(true);
                EditorUtility.SetDirty(element);
                EditorUtility.SetDirty(element.tile);
            }            
        }
    }
}
