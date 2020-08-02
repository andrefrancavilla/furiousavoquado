using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject needTaxiIndicator;
    public GameObject mustDriveHereIndicator;
    [Header("Readonly")]
    public GUI gui;
    public CameraController camController;
    public List<Building> currentlyActiveRequests;
    public bool isSelectingBuilding;

    private Building _destinationBuilding;
    private Building _originBuilding;
    
    // Start is called before the first frame update
    void Start()
    {
        gui = FindObjectOfType<GUI>();
        BuildingManager.GetBuildingById(1).RequestTaxi();
        BuildingManager.GetBuildingById(3).RequestTaxi();
        camController = FindObjectOfType<CameraController>();
    }

    public void PickUpPassenger(Building fromBuilding)
    {
        for (int i = 0; i < currentlyActiveRequests.Count; i++)
        {
            if (currentlyActiveRequests[i] == fromBuilding)
            {
                currentlyActiveRequests[i].buildingSelected = true;
                
                _originBuilding = currentlyActiveRequests[i];
                _destinationBuilding = BuildingManager.GetBuildingById(currentlyActiveRequests[i].buildingIdToDriveTowards);
                
                _destinationBuilding.driveTowardsIndicatorInstance = Instantiate(mustDriveHereIndicator, _destinationBuilding.requestIconPosition.position, transform.rotation, gui.transform);
                _destinationBuilding.mustDriveTowards = true;
            }
            
            currentlyActiveRequests[i].requestIndicatorInstance.SetActive(false);
        }

        isSelectingBuilding = true;
    }

    public void DropOffPassenger(Building toBuilding)
    {
        Destroy(_originBuilding.requestIndicatorInstance);
        Destroy(_destinationBuilding.driveTowardsIndicatorInstance);
        
        _originBuilding.buildingSelected = false;
        _destinationBuilding.mustDriveTowards = false;
        currentlyActiveRequests.Remove(_originBuilding);
        
        for (int i = 0; i < currentlyActiveRequests.Count; i++)
        {
            currentlyActiveRequests[i].requestIndicatorInstance.SetActive(true);
        }

        isSelectingBuilding = false;
    }

    public void AddActiveRequest(Building requestingBuilding, Vector3 indicatorPosition)
    {
        currentlyActiveRequests.Add(requestingBuilding);
        requestingBuilding.requestIndicatorInstance = Instantiate(needTaxiIndicator, indicatorPosition, transform.rotation, gui.transform);
        requestingBuilding.requestIndicatorInstance.SetActive(!isSelectingBuilding); //Must not show if a building is already selected
    }
}
