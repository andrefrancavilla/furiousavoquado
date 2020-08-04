using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

public static class BuildingManager
{
    public static List<Building> allBuildings = new List<Building>();
    public static GameplayManager gameplayManager;
    private static int lastBuildingID;

    static BuildingManager()
    {
        gameplayManager = GameObject.FindObjectOfType<GameplayManager>();
    }
    public static int GetNewGetBuildingId(Building building)
    {
        lastBuildingID++;
        allBuildings.Add(building);
        
        return lastBuildingID;
    }

    public static int GetPassengerDestinationBuildingId(int buildingThatIsRequestingId)
    {
        int randomId = 0;

        do
        {
            randomId = Random.Range(1, lastBuildingID);
        } 
        while (randomId == buildingThatIsRequestingId || GetBuildingById(randomId).requestingTaxi);

        return randomId;
    }

    public static Building GetBuildingById(int id)
    {
        for (int i = 0; i < allBuildings.Count; i++)
        {
            if (allBuildings[i].buildingId == id)
                return allBuildings[i];
        }

        Debug.LogError($"Could not find building with ID {id}");
        return null;
    }
}

public enum BuildingOrientation
{
    Bottom = 90,
    Top = -90,
    Left = 180,
    Right = 0
}
