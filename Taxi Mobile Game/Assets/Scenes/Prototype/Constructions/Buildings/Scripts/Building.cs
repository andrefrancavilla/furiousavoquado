using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [Header("Id")]
    public int buildingId;
    
    [Header("Configuration")]
    public BuildingOrientation orientation;
    public Transform requestIconPosition;
    
    [Header("Readonly")]
    public bool requestingTaxi;
    public bool mustDriveTowards;
    public int buildingIdToDriveTowards;
    public bool buildingSelected;
    public GameObject requestIndicatorInstance;
    public GameObject driveTowardsIndicatorInstance;
    
    // Start is called before the first frame update
    void Awake()
    {
        buildingId = BuildingManager.GetNewGetBuildingId(this);
        
        transform.rotation = Quaternion.Euler(new Vector3(0, (int)orientation, 0));
    }

    public void RequestTaxi()
    {
        buildingSelected = true;
        requestingTaxi = true;
        buildingIdToDriveTowards = BuildingManager.GetBuildingIdToBringTaxiAt(buildingId);
        BuildingManager.GetBuildingById(buildingIdToDriveTowards).SetAsDestination();
        BuildingManager.gameplayManager.AddActiveRequest(this, requestIconPosition.position);
    }

    public void SetAsDestination()
    {
        mustDriveTowards = true;
    }

    private void Update()
    {
        if (requestIndicatorInstance)
        {
            requestIndicatorInstance.transform.position = BuildingManager.gameplayManager.camController.mainCamera.WorldToScreenPoint(requestIconPosition.position);
        }

        if (driveTowardsIndicatorInstance)
        {
            driveTowardsIndicatorInstance.transform.position = BuildingManager.gameplayManager.camController.mainCamera.WorldToScreenPoint(requestIconPosition.position);
        }
    }
}
