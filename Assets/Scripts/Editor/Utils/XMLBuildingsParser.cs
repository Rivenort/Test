using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Experimental.AI;

/// <summary>
/// Custom parser for for buildings data.
/// @author Rivenort
/// </summary>
public class XMLBuildingsParser
{
    const string TAG_ID = "ID";
    const string TAG_BUILDING_TYPE = "Type";
    const string TAG_NAME = "Name";
    const string TAG_DESC = "Desc";
    const string TAG_PREFAB = "Prefab";
    const string TAG_WARE_TYPE = "Produces";
    const string TAG_PROD_FOOD = "ProdFood";
    const string TAG_PROD_RESEARCH = "ProdResearch";
    const string TAG_SUPP_SETTLERS = "SuppSettlers";
    const string TAG_COST_FOOD = "CostFood";
    const string TAG_COST_RESEARCH = "CostResearch";
    const string TAG_COST_FACTOR = "CostFactor";
    const string TAG_BUILD_TIME = "BuildTime";
    const string TAG_AVAILABLE = "Available";
    const string FTAG_DEPENDENCY = "dep";
    const string FTAG_DEPENDENCY_VALUE = "dep{0}val";
    internal enum BuildingType
    {
        HOUSE,
        PRODUCTION
    }

    /** Dependency between two buildings. When the first building achivies required mood level
     on all of its instances, then the next Building is gonna be unlocked. 
     The following class is only used for sake of parising.   
     */
    internal class BuildingDependency
    {
        internal int fromBuilding;
        internal int toBuilding;
        internal int reqMood;

        internal BuildingDependency() { }
        internal BuildingDependency(int fromBuilding,
                                    int toBuilding,
                                    int reqMood){
            this.fromBuilding = fromBuilding;
            this.toBuilding = toBuilding;
            this.reqMood = reqMood;
        }
    }



    public static DAT_BuildingTemplates Parse(string filename)
    {        
        List<DAT_BuildingHouse> buildingsHouse = new List<DAT_BuildingHouse>();
        List<DAT_BuildingProd> buildingsProd = new List<DAT_BuildingProd>();

        // load assets from "/Resources"
        TextAsset xmlAsset = Resources.Load<TextAsset>(filename);

        // Parse into xml document
        XDocument doc = XDocument.Parse(xmlAsset.text);
        // Get all elements inside building/Sheet1 tag
        IEnumerable<XElement> elems = doc.Element("building").Element("Sheet1").Elements();

        List<BuildingDependency> dependencies = new List<BuildingDependency>();

        foreach (XElement e in elems) {

            int id = 0;
            BuildingType type = BuildingType.PRODUCTION;
            string name = "Building";
            string desc = "Here goes a description.";
            string prefabName = "store";
            DAT_Ware.Type ware = DAT_Ware.Type.NOTHING;
            int prodFood = 0;
            int prodResearch = 0;
            int suppSettlers = 0;
            int costFood = 0;
            int costResearch = 0;
            float costFactor = 1f;
            int buildTime = 2;
            bool available = false;
            List<BuildingDependency> tempDeps = new List<BuildingDependency>();

            foreach (XElement prop in e.Elements())
            {
                if (prop.Name.ToString().Equals(TAG_ID))
                {
                    id = Int32.Parse(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_BUILDING_TYPE))
                {
                    string bType = prop.Value.ToUpper();
                    if (bType.Equals("HOUSE"))
                    {
                        type = BuildingType.HOUSE;
                    }
                    else if (bType.Equals("PRODUCTION"))
                    {
                        type = BuildingType.PRODUCTION;
                    }
                    else
                    {
                        type = BuildingType.PRODUCTION;
                    }

                }
                else if (prop.Name.ToString().Equals(TAG_NAME))
                {
                    name = prop.Value;
                }
                else if (prop.Name.ToString().Equals(TAG_DESC))
                {
                    desc = prop.Value;
                }
                else if (prop.Name.ToString().Equals(TAG_PREFAB))
                {
                    prefabName = prop.Value;
                }
                else if (prop.Name.ToString().Equals(TAG_WARE_TYPE))
                {
                    ware = DAT_Ware.ToType(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_PROD_FOOD))
                {
                    prodFood = Int32.Parse(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_PROD_RESEARCH)) 
                {
                    prodResearch = Int32.Parse(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_COST_FOOD))
                {
                    costFood = Int32.Parse(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_COST_RESEARCH))
                {
                    costResearch = Int32.Parse(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_COST_FACTOR))
                {
                    costFactor = float.Parse(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_BUILD_TIME))
                {
                    buildTime = Int32.Parse(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_AVAILABLE))
                {
                    available = Boolean.Parse(prop.Value);
                }
                else if (prop.Name.ToString().Equals(TAG_SUPP_SETTLERS))
                {
                    suppSettlers = Int32.Parse(prop.Value);
                }
                else // dependency
                {
                    string tag = prop.Name.ToString();
                    int r;
                    if (!Int32.TryParse(prop.Value, out r)) continue;


                    // <dep{0}>
                    if (tag.StartsWith("dep") && !tag.EndsWith("val"))
                    {

                        int depId = Int32.Parse(tag.Substring(3));
                        bool found = false;
                        for (int i = 0; i < tempDeps.Count; i++)
                        {
                            if (depId - 1 == i)
                            {
                                tempDeps[i].fromBuilding = Int32.Parse(prop.Value);
                                tempDeps[i].toBuilding = id;
                                found = true;
                                break;
                            }
                        }
                        // if temp dependency wasn't created
                        if (!found)
                        {
                            tempDeps.Add(new BuildingDependency(Int32.Parse(prop.Value), id, 0));
                        }
                    }

                    // <dep{0}val>
                    if (tag.StartsWith("dep") && tag.EndsWith("val"))
                    {
                        int depId = Int32.Parse(tag.Substring(3, tag.Length - 6));
                        bool found = false;
                        for (int i = 0; i < tempDeps.Count; i++)
                        {
                            if (depId - 1 == i)
                            {
                                tempDeps[i].reqMood = Int32.Parse(prop.Value);
                                found = true;
                                break;
                            }
                        }
                        // if temp dependency wasn't created
                        if (!found)
                        {
                            tempDeps.Add(new BuildingDependency(0, 0, Int32.Parse(prop.Value)));
                        }
                    }
                }
                
            }

            foreach (BuildingDependency dep in tempDeps)
            {
                dependencies.Add(dep);
            }



            switch (type)
            {
                case BuildingType.HOUSE:
                    {
                        DAT_BuildingHouse newBuilding = new DAT_BuildingHouse();
                        newBuilding.id = id;
                        newBuilding.name = name;
                        newBuilding.desc = desc;
                        newBuilding.prefabName = prefabName;
                        newBuilding.costFood = costFood;
                        newBuilding.costResearch = costResearch;
                        newBuilding.costFactor = costFactor;
                        newBuilding.buildingTime = buildTime;
                        newBuilding.availableOnStart = available;
                        newBuilding.settlersSupplied = suppSettlers;
                        newBuilding.dependencies = new List<DAT_Building.BuildDependency>();

                        buildingsHouse.Add(newBuilding);
                    }
                    break;
                case BuildingType.PRODUCTION:
                    {
                        DAT_BuildingProd newBuilding = new DAT_BuildingProd();
                        newBuilding.id = id;
                        newBuilding.name = name;
                        newBuilding.desc = desc;
                        newBuilding.prefabName = prefabName;
                        newBuilding.costFood = costFood;
                        newBuilding.costResearch = costResearch;
                        newBuilding.costFactor = costFactor;
                        newBuilding.buildingTime = buildTime;
                        newBuilding.availableOnStart = available;
                        newBuilding.dependencies = new List<DAT_Building.BuildDependency>();

                        newBuilding.producedFood = prodFood;
                        newBuilding.producedResearch = prodResearch;
                        newBuilding.producedWare = ware;

                        buildingsProd.Add(newBuilding);
                    }
                    break;
            }
        }

        foreach (BuildingDependency dep in dependencies)
        {
            DAT_Building temp = null;
            foreach (DAT_Building building in buildingsProd)
            {
                if (building.id == dep.fromBuilding)
                {
                    temp = building;
                    break;
                }
            }
            foreach (DAT_Building building in buildingsHouse)
            {
                if (building.id == dep.fromBuilding)
                {
                    temp = building;
                    break;
                }
            }
            if (temp == null)
            {
                Debug.Log("Incorrect dependency definition.");
                break;
            }

            DAT_Building.BuildDependency buildDependency = new DAT_Building.BuildDependency();
            buildDependency.unlocksBuilding = dep.toBuilding;
            buildDependency.requiredMood = dep.reqMood;
            temp.dependencies.Add(buildDependency);
        }


        DAT_BuildingTemplates buildings = ScriptableObject.CreateInstance<DAT_BuildingTemplates>();
        buildings.house = buildingsHouse;
        buildings.production = buildingsProd;

        return buildings;
    }
}
