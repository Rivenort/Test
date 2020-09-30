using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UT_BuildingMechanics
{
    internal class BuildingStats
    {
        internal int createdInstances;
        internal int availability; // must be equal to the number of inConnections
        internal int overallMood;
        internal int buildingLevel = int.MaxValue;

        internal DAT_Building data;
    }


    private UT_DirectedGraph<BuildingStats> m_graphBuildings;

    private int m_currentBuildingsLevel = 0;

    public UT_BuildingMechanics(List<DAT_Building> buildings)
    {
        m_graphBuildings = new UT_DirectedGraph<BuildingStats>();

        // add nodes
        foreach (DAT_Building building in buildings)
        {
            BuildingStats temp = new BuildingStats();
            temp.data = building;
            temp.availability = 0;

            m_graphBuildings.AddNode(temp);
        }

        // set connections
        m_graphBuildings.Foreach((UT_Node<BuildingStats> node) =>
        {
            BuildingStats stats = node.GetData();

            foreach (DAT_Building.BuildDependency dep in stats.data.dependencies) {
                UT_Node<BuildingStats> temp = GetNode(dep.unlocksBuilding);
                m_graphBuildings.AddConnection(node, temp);
            }
        });

        GetPathLengths(5);
        GetPathLengths(1);
        GetPathLengths(33);
        m_graphBuildings.Foreach((UT_Node<BuildingStats> node) => {
            Debug.Log(node.GetData().data.name + " level: " + node.GetData().buildingLevel);
        });
    }

    private UT_Node<BuildingStats> GetNode(int id)
    {
        List<UT_Node<BuildingStats>> nodes = m_graphBuildings.GetNodes();
        foreach (UT_Node<BuildingStats> node in nodes)
        {
            if (node.GetData().data.id == id)
                return node;
        }
        return null;
    }

    public int GetCount(int id)
    {
        UT_Node<BuildingStats> node = GetNode(id);
        return node.GetData().createdInstances;
    }

    public List<DAT_Building> GetAvailableBuildings()
    {
        List<DAT_Building> res = new List<DAT_Building>();

        m_graphBuildings.Foreach((UT_Node<BuildingStats> node) =>
        {
            BuildingStats stats = node.GetData();
            if (stats.availability == node.GetInAdjacent().Count)
                res.Add(stats.data);
        });

        return res;
    }

    public List<DAT_Ware.Type> GetAvailableWares(int a, int b)
    {
        List<DAT_Ware.Type> wares = new List<DAT_Ware.Type>();
        m_graphBuildings.Foreach((UT_Node<BuildingStats> node) => {
            BuildingStats stats = node.GetData();
            if (stats.data is DAT_BuildingProd && stats.buildingLevel >= a && stats.buildingLevel <= b)
            {
                wares.Add(((DAT_BuildingProd)stats.data).producedWare);
            }
        });

        return wares;
    }

    public List<DAT_Ware.Type> GetAvailableWares()
    {
        return GetAvailableWares(m_currentBuildingsLevel, m_currentBuildingsLevel + 1);
    }

    public void UpdatePerDay()
    {
        m_graphBuildings.Foreach((UT_Node<BuildingStats> node) =>
        {
            node.GetData().overallMood = 0;
            node.GetData().availability = 0;
            node.GetData().createdInstances = 0;
        });

        List<IBuilding> buildings = M_BuildingManager.SGetBuildings();

        foreach (IBuilding building in buildings)
        {
            if (building is IBuildingHouse)
            {
                GetNode(building.GetData().id).GetData().createdInstances++;
                continue;
            }
            if (!(building is IBuildingProduction)) continue;

            IBuildingProduction prod = (IBuildingProduction) building;
            UT_Node<BuildingStats> node = GetNode(building.GetData().id);
            node.GetData().overallMood += prod.GetMood();
            node.GetData().createdInstances++;

            if (prod.IsWorking())
                M_WaresManager.SAddProductiveBuilding(prod.GetId(), prod.GetProducedWareType());
            else
                M_WaresManager.SRemoveProductiveBuilding(prod.GetId(), prod.GetProducedWareType());
        }

        m_graphBuildings.Foreach((UT_Node<BuildingStats> node) =>
        {
            BuildingStats stats = node.GetData();
            foreach (DAT_Building.BuildDependency dep in stats.data.dependencies)
            {
                if (stats.overallMood > dep.requiredMood)
                {
                    UT_Node<BuildingStats> tempNode = GetNode(dep.unlocksBuilding);
                    tempNode.GetData().availability += 1;
                    if (tempNode.GetData().availability == tempNode.GetAdjacent().Count
                        && m_currentBuildingsLevel < tempNode.GetData().buildingLevel)
                        m_currentBuildingsLevel = tempNode.GetData().buildingLevel;
                        
                }
                    
            }
        });
    }

    public void GetPathLengths(int start)
    {
        List<UT_Node<BuildingStats>> m_nodes = m_graphBuildings.GetNodes();

        int level = 0;
        bool[] visited = new bool[m_nodes.Count];

        Queue<UT_Node<BuildingStats>> queue = new Queue<UT_Node<BuildingStats>>();

        UT_Node<BuildingStats> startNode = GetNode(start);
        visited[m_nodes.IndexOf(startNode)] = true;
        queue.Enqueue(startNode);

        startNode.GetData().buildingLevel = level;

        while (queue.Count > 0)
        {
            UT_Node<BuildingStats> temp = queue.Dequeue();

            if (temp.GetData().buildingLevel > level)
                level++;

            List<UT_Node<BuildingStats>> adj = temp.GetAdjacent();
            foreach (UT_Node<BuildingStats> node in adj)
            {
                int index = m_nodes.IndexOf(node);
                if (!visited[index])
                {
                    visited[index] = true;
                    node.GetData().buildingLevel = level + 1;
                    queue.Enqueue(node);
                }
            }
        }

    }

    public UT_Pair<int, int> GetBuildingCost(int id)
    {
        BuildingStats stats = GetNode(id).GetData();
        

        int requiredFood = stats.data.costFood;
        int requiredResearch = stats.data.costResearch;
        int count = stats.createdInstances;

        float temp = requiredFood;
        for (int i = 0; i < count; i++)
        {
            temp = temp * stats.data.costFactor;
        }
        requiredFood = (int)temp;
        temp = requiredResearch;
        for (int i = 0; i < count; i++)
        {
            temp = (temp * stats.data.costFactor);
        }
        requiredResearch = (int)temp;
        return new UT_Pair<int, int>(requiredFood, requiredResearch);
    }
}
