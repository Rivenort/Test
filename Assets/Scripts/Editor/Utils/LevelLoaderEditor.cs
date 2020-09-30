using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// @author Rivenort
/// </summary>
public class LevelLoaderEditor : EditorWindow
{
    public static readonly Vector2 SIZE = new Vector2(200, 100);

    DAT_Game m_data = null;

    [MenuItem(itemName: EditorPaths.MENU_UTILS + "LevelLoader")]
    private static void OpenWindow()
    {
        LevelLoaderEditor window = GetWindow<LevelLoaderEditor>();
        window.maxSize = SIZE;
        window.minSize = SIZE;
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        m_data = (DAT_Game) EditorGUILayout.ObjectField(m_data, typeof(DAT_Game));
        if (GUILayout.Button("Load"))
        {
            if (m_data != null)
            {
               // M_GameHelper.LoadLevelInEditor(m_data);
            }
        }
        GUILayout.EndHorizontal();
    }
}
