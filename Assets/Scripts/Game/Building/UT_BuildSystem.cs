using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UT_BuildSystem
{
    

    private GameObject m_tempBuildingObj = null;
    private IBuilding m_tempBuildingComp = null;
    private GameObject m_currentMiddleTile = null;
    private List<Tile> m_tilesHighlighted;

    private float m_buttonYOffset;
    private bool m_dragging = false;

    private Action<IBuilding> m_onBuildingPlacedAction;

    public UT_BuildSystem(Action<IBuilding> OnBuildingPlacedAction)
    {
        m_tilesHighlighted = new List<Tile>();
        m_onBuildingPlacedAction = OnBuildingPlacedAction;
    }

    public void FixedUpdate()
    {
        if (m_tempBuildingObj == null) return;

        Vector3 pos = m_tempBuildingObj.transform.position;
        pos.y -= 0.1f;
        GameObject collider = M_GameHelper.SGetObjectAtWorldPoint(pos);
        if (collider != null)
        {
            if (m_currentMiddleTile != collider.transform.parent.gameObject || m_currentMiddleTile == null)
            {
                foreach (Tile obj in m_tilesHighlighted)
                {
                    obj.HighlightOccupied(false);
                }
                m_tilesHighlighted.Clear();
                m_currentMiddleTile = collider.transform.parent.gameObject;

                Tile tile = collider.transform.parent.gameObject.GetComponent<Tile>();
                if (tile == null)
                    return;

                BuildingPhysics physics = m_tempBuildingComp.GetPhysics();
                bool[] area = physics.GetArea();
                int middleI = physics.GetWidth() / 2;
                int middleJ = physics.GetHeight() / 2;

                for (int i = 0; i < physics.GetWidth(); i++)
                {
                    for (int j = 0; j < physics.GetHeight(); j++)
                    {
                        int tempI = tile.i - (middleI - i);
                        int tempJ = tile.j - (middleJ - j);
                        Tile tempTile = M_MapManager.SGetTileObject(tempI, tempJ).GetComponent<Tile>();
                        tempTile.HighlightOccupied(true);
                        m_tilesHighlighted.Add(tempTile);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Starts building process.
    /// Building is supposed to be placed.
    /// </summary>
    public void StartBuilding(DAT_Building buildingData)
    {
        // prepare position (middle of a screen) and instantiate
        Vector3 pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));
        pos.y = M_MapManager.SGetHighYDepthValue(pos.z); // set appropriate depth value. (Building has to be above every other obj)
        // Instantiate new building in the TEMP group.
        m_tempBuildingObj = buildingData.InstantiateBuilding(pos, M_GameHelper.Group.TEMP);
        m_tempBuildingComp = m_tempBuildingObj.GetComponent<IBuilding>();
        // Get build button and activate it.
        m_tempBuildingComp.GetBuildButton().GetComponent<Button>().onClick.AddListener(OnBuildingBuildIconClick);
        m_tempBuildingComp.GetBuildButton().SetActive(true);

        M_MiscManager.RemoveListenersCancelButton();
        M_MiscManager.AddListenerCancelButton(QuitBuilding);
        M_MiscManager.ShowCancelButton(true);
    }

    /// <summary> Function called when Building is supposed to be placed. </summary>
    private void OnBuildingBuildIconClick()
    {
        // check whether the place is not occupied.
        foreach (Tile tile in m_tilesHighlighted)
        {
            if (tile.IsOccupied()) return;
        }

        UT_Pair<int, int> cost = M_BuildingManager.SGetBuildingCost(m_tempBuildingComp.GetData().id);
        // Apply cost to the resources
        M_InGameResourcesManager.SAddFood(-cost.first);
        M_InGameResourcesManager.SAddResearch(-cost.second);

        // Mark highlighted tiles as occupied
        foreach (Tile tile in m_tilesHighlighted)
        {
            tile.SetOccupied(true);
            tile.Highlight(false);
        }
        // set final building position
        Vector3 pos = m_currentMiddleTile.transform.position;
        pos.y = M_MapManager.SGetYDepthValue(pos.z);
        m_tempBuildingObj.transform.position = pos;
        // Move to the BUILDINGS group
        M_GameHelper.AddToGroup(m_tempBuildingObj, M_GameHelper.Group.BUILDINGS);

        m_tempBuildingComp.SetActionOnBuildingFinished(ActionOnBuildingFinished);
        // Start in-game building process
        m_tempBuildingComp.StartBuilding();


        m_onBuildingPlacedAction(m_tempBuildingComp);

        // reset values
        m_tempBuildingObj.GetComponent<IBuilding>().GetBuildButton().SetActive(false);
        m_tempBuildingObj = null;
        m_tempBuildingComp = null;
        m_currentMiddleTile = null;
        m_tilesHighlighted.Clear();
        M_MiscManager.ShowCancelButton(false);
    }

    public void QuitBuilding()
    {
        if (m_tempBuildingObj != null)
        {
            m_tempBuildingComp = null;
            M_WorldUIManager.ShowSmallBuildButton(false);
            M_GameHelper.Destroy(m_tempBuildingObj);
            m_tempBuildingObj = null;
            M_MiscManager.ShowCancelButton(false);
            foreach (Tile tile in m_tilesHighlighted)
            {
                tile.Highlight(false);
            }
        }
    }

    public bool IsBuildingStarted()
    {
        if (m_tempBuildingObj == null) 
            return false;
        return true;
    }

    public void Clear()
    {
        m_tempBuildingObj = null;
        m_tempBuildingComp = null;
        m_currentMiddleTile = null;
        m_tilesHighlighted = new List<Tile>();
    }

    private void ActionOnBuildingFinished(IBuilding building)
    {
        if (building is IBuildingHouse)
        {
            IBuildingHouse house = ((IBuildingHouse)building);
            int count = house.GetProvidedSettlers();
           
            for (int i = 0; i < count; i++)
            {
                house.AssignSettler(M_SettlersManager.SCreateSettler(house.GetId()));
            }
          
        }
    }
}
