using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Should be one instance of the following component.
/// Handles main behaviour of the game.
/// @author Dominik
/// </summary>
public class M_GameHelper : MonoBehaviour
{
    private static M_GameHelper s_instance = null;

    public const string EDITOR_MENU_ITEM = "WelcomeInMyCave/";

    public const string TAG_LEVEL = "Level";

    // -------------------------------------------------------
    // Managers
    // -------------------------------------------------------
    private CameraController         m_camController = null;
    private M_InGameResourcesManager m_resourcesManager = null;
    private M_BuildingManager        m_buildingManager = null;
    private M_MapManager             m_mapManager = null;
    private M_SettlersManager        m_settlersManager = null;
    private M_InputManager           m_inputManager = null;
    private M_WaresManager           m_waresManager = null;
    private M_SaveManager            m_saveManager = null;

    // InGameResourcesManager
    public TMP_Text dayText;
    public TMP_Text foodText;
    public TMP_Text researchText;
    //

    // -------------------------------------------------------
    // Groups
    // -------------------------------------------------------
    public const string GROUP_LEVEL       = "Level";
    public const string GROUP_TEMP        = "Temp"; 
    public const string GROUP_BUILDINGS   = "Buildings"; 
    public const string GROUP_TERRAIN     = "Terrain";
    public const string GROUP_ENVIRONMENT = "Environment";

    public enum Group
    {
        LEVEL,
        TERRAIN,
        BUILDINGS,
        TEMP
    }

    private GameObject m_levelObject;
    private GameObject m_tempGroup;
    private GameObject m_buildingsGroup;
    private GameObject m_terrainGroup;
    //--------------
    // Utilities
    //--------------

    public DAT_Game defaultGameData;

    private bool m_pause = false;
    private bool m_levelPause = false;

    public TMP_Text outputTextObj;
    public static string debugText;


    void Start()
    {
        Debug.Log("GameHelper -> Start()");
        s_instance = this;

        m_camController = Camera.main.GetComponent<CameraController>();
        if (m_camController == null)
            Debug.LogWarning("Couldn't get CameraController from Camera.main!");

        if (dayText == null || foodText == null || researchText == null)
            Debug.LogWarning("One of the TMP_Text objects is not initialized.");
        else
            m_resourcesManager = M_InGameResourcesManager.GetInstance(foodText, dayText, researchText);

        m_buildingManager = M_BuildingManager.Instance;
        m_mapManager = M_MapManager.Instance;
        m_settlersManager = M_SettlersManager.Instance;
        m_inputManager = M_InputManager.Instance;
        m_waresManager = M_WaresManager.Instance;
        m_saveManager = M_SaveManager.Instance;

        // Load level 
        GameObject menuManager = GameObject.FindGameObjectWithTag("MenuManager");
        if (menuManager != null)
        {
            SceneLoader sceneLoader = menuManager.GetComponent<SceneLoader>();
            if (sceneLoader != null)
            {
                if (sceneLoader.gameData == null)
                    LoadLevel(defaultGameData);
                else
                    LoadLevel(sceneLoader.gameData);

                    
                if (sceneLoader.saveId > 0)
                    LoadSave(sceneLoader.saveId);
            } else
            {
                Debug.LogWarning("Cannot find " + typeof(SceneLoader).Name + " component!");
            }
        }
        else
        { // Only if we started from Game scene, because MenuManager hasn't been initialized.
            if (defaultGameData != null)
            {
                Debug.LogWarning("Loading using default data .");
                LoadLevel(defaultGameData);
            } else
            {
                Debug.LogWarning("Couldn't find either MenuManager or default DAT_Game!");
            }
                
        }
        ResumeGame();
    }


    void Update()
    {
        outputTextObj.text = debugText;
        if (!m_levelPause)
        {
            m_inputManager.Update();
            m_resourcesManager.Update();

            m_buildingManager.Update();
        } 

    }

    private void FixedUpdate()
    {

        if (!m_levelPause)
        {
            m_inputManager.FixedUpdate();
            m_buildingManager.FixedUpdate();
        }
    }
    /// <summary>
    /// Setups references to the groups of gameobjects.
    /// </summary>
    private void SetupObjectGroups()
    {
        m_levelObject = GameObject.FindGameObjectWithTag(TAG_LEVEL);

        if (m_levelObject == null)
            Debug.LogWarning("levelObject instance is not set!");
        else
        {
            for (int i = 0; i < m_levelObject.transform.childCount; i++)
            {
                GameObject temp = m_levelObject.transform.GetChild(i).gameObject;
                if (temp.name.Equals(GROUP_TEMP))
                    m_tempGroup = temp;
                else if (temp.name.Equals(GROUP_BUILDINGS))
                    m_buildingsGroup = temp;
                else if (temp.name.Equals(GROUP_TERRAIN))
                    m_terrainGroup = temp;
                else
                    Debug.LogWarning("Unknown group name!");
            }
        }
    }

    private GameObject GetObjectAtScreenPoint(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.collider.gameObject;
        return null;
    }

    /// <param name="updating"> true if camera should evaluate input events.</param>
    public static void SetCameraMovement(bool updating)
    {
        if (s_instance != null && s_instance.m_camController != null)
        {
            s_instance.m_camController.OverallMovement = updating;
        }
    }

    /// <param name="updating"> true if camera should be controlled through arrows.</param>
    public static void SetCameraMovementArrows(bool updating)
    {
        if (s_instance != null && s_instance.m_camController != null)
        {
            s_instance.m_camController.ArrowsMovement = updating;
        }
    }

    /// <param name="updating"> true if camera should be controlled through touch.</param>
    public static void SetCameraMovementTouch(bool updating)
    {
        if (s_instance != null && s_instance.m_camController != null)
        {
            s_instance.m_camController.TouchMovement = updating;
        }
    }


    public void PauseLevel()
    {
        m_levelPause = true;
        m_camController.OverallMovement = false;
        m_buildingManager.PauseBuildings(true);
    }

    public void ResumeLevel()
    {
        m_levelPause = false;
        m_camController.OverallMovement = true;
        m_buildingManager.PauseBuildings(false);
    }

    public static void SResumeLevel()
    {
        if (s_instance != null)
        {
            s_instance.ResumeLevel();
        }
    }

    public static void SPauseLevel()
    {
        if (s_instance != null)
        {
            s_instance.PauseLevel();
        }
    }

    public void PauseGame()
    {
        m_pause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        m_pause = false;
        Time.timeScale = 1;
    }

    public static void SPauseGame()
    {
        if (s_instance != null)
            s_instance.PauseGame();
    }

    public static void SResumeGame()
    {
        if (s_instance != null)
            s_instance.ResumeGame();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void QuitScene()
    {
        m_buildingManager.Clear();
        m_settlersManager.Clear();
        m_inputManager.Clear();
        m_mapManager.Clear();
        m_resourcesManager.Clear();
    }

    /// <summary>
    /// [Safe in edit mode]
    /// </summary>
    public static void AddToGroup(GameObject obj, Group group)
    {
        GameObject groupObj = null;
        if (s_instance != null)
        {
            switch (group)
            {
                case Group.TEMP:
                    groupObj = s_instance.m_tempGroup;
                    break;
                case Group.BUILDINGS:
                    groupObj = s_instance.m_buildingsGroup;
                    break;
                case Group.TERRAIN:
                    groupObj = s_instance.m_terrainGroup;
                    break;
            }
        }
        if (groupObj == null)
        {
            groupObj = SGetGroup(group);
        }
        if (groupObj == null)
        {
            Debug.LogWarning(String.Format("Couldn't add object into group {0}!", group));
            return;
        }
        obj.transform.parent = groupObj.transform;
    }



    /// <summary>
    /// Instantiates an object and puts it into a Temp objects group.
    /// </summary>
    public static GameObject CreateInstanceInTemp(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        GameObject temp =  Instantiate(prefab, pos, rotation);
        if (s_instance == null)
        {
            GameObject group = SGetGroup(Group.TEMP);
            if (group == null)
            {
                Debug.LogWarning("Couldn't add object into Temp group!");
                return temp;
            }
            temp.transform.parent = group.transform;
        }
        else if (s_instance.m_tempGroup == null)
        {
            GameObject group = SGetGroup(Group.TEMP);
            if (group == null)
            {
                Debug.LogWarning("Couldn't add object into Temp group!");
                return temp;
            }
            temp.transform.parent = group.transform;
        }
        else
            temp.transform.parent = s_instance.m_tempGroup.transform;
        return temp;
    }

    /// <summary>
    /// Instantiates an object and puts it into a Buildings objects group.
    /// </summary>
    public static GameObject CreateInstanceInBuildings(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        GameObject temp = Instantiate(prefab, pos, rotation);
        if (s_instance == null)
        {
            GameObject group = SGetGroup(Group.BUILDINGS);
            if (group == null)
            {
                Debug.LogWarning("Couldn't add object into Buildings group!");
                return temp;
            }
            temp.transform.parent = group.transform;
        }
        else if (s_instance.m_buildingsGroup == null)
        {
            GameObject group = SGetGroup(Group.BUILDINGS);
            if (group == null)
            {
                Debug.LogWarning("Couldn't add object into Buildings group!");
                return temp;
            }
            temp.transform.parent = group.transform;
        } else
            temp.transform.parent = s_instance.m_buildingsGroup.transform;
        return temp;
    }

    /// <summary>
    /// Instantiates an object and puts it into a Terrain objects group.
    /// </summary>
    public static GameObject CreateInstanceInTerrain(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        GameObject temp = Instantiate(prefab, pos, rotation);
        if (s_instance == null || s_instance.m_terrainGroup == null)
        {
            GameObject group = SGetGroup(Group.TERRAIN);
            if (group == null)
            {
                Debug.LogWarning("Couldn't add object into Terrain group!");
                return temp;
            }
            temp.transform.parent = group.transform;
        }
        else if (s_instance.m_terrainGroup == null)
        {
            GameObject group = SGetGroup(Group.TERRAIN);
            if (group == null)
            {
                Debug.LogWarning("Couldn't add object into Terrain group!");
                return temp;
            }
            temp.transform.parent = group.transform;
        }
            temp.transform.parent = s_instance.m_terrainGroup.transform;
        return temp;
    }

    /// <summary> Should be called in the FixedUpdate(). </summary>
    /// <param name="point">Screen point</param>
    /// <returns>collider object if collides</returns>
    public static GameObject SGetObjectAtScreenPoint(Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(point);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            return hit.collider.gameObject;
        return null;
    }

    public static GameObject SGetObjectAtWorldPoint(Vector3 point)
    {
        Ray ray = new Ray(point, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    public static void LoadLevel(DAT_Game gameData)
    {
        Debug.Log("LoadLevel()");
        Debug.Log("[Loading level]");
        s_instance.SetupObjectGroups();

        Debug.Log("[Loading level]: Setting game data...");
        s_instance.m_resourcesManager.ResourcesData = Instantiate(gameData.resources);
        s_instance.m_buildingManager.SetBuildingTemplates(gameData.buildings);
        s_instance.m_settlersManager.SetNamesData(gameData.settlerNames);
        s_instance.m_settlersManager.SetPortraitsData(gameData.settlerPortraits);

        Debug.Log("[Loading level]: Setup a terrain...");
        s_instance.m_mapManager.SetTiledMap(s_instance.m_levelObject.GetComponent<Level>().tiledMap);


        Debug.Log("[Loading level]: Done.");
    }

    /// <summary>
    /// [Safe in edit mode]
    /// Returns a gameobject that is a contiainer for a specified group of objects.
    /// </summary>
    public static GameObject SGetGroup(Group group)
    {
        GameObject level = GameObject.FindGameObjectWithTag(TAG_LEVEL);
        if (level == null)
        {
            Debug.LogWarning("Couldn't find any Level!");
            return null;
        }
        else if (group == Group.LEVEL)
            return level;

        for (int i = 0; i < level.transform.childCount; i++)
        {
            GameObject temp = level.transform.GetChild(i).gameObject;
            if (temp.name.Equals(GROUP_TEMP) && group == Group.TEMP)
                return temp;
            else if (temp.name.Equals(GROUP_BUILDINGS) && group == Group.BUILDINGS)
                return temp;
            else if (temp.name.Equals(GROUP_TERRAIN) && group == Group.TERRAIN)
                return temp;
        }
        Debug.LogWarning("Couldn't find specified Group!");
        return null;
    }


    public void SaveGame(int id)
    {
        m_saveManager.Save(string.Format("save{0}.sav", id));
    }

    public void LoadSave(int id)
    {
        m_saveManager.Load(string.Format("save{0}.sav", id));
    }
}
