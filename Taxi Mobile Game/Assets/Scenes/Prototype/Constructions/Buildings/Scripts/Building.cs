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
    public Transform pedestrianSpawnPosition;
    
    [Header("Readonly")]
    public bool requestingTaxi;
    public bool mustDriveTowards;
    public int buildingIdToDriveTowards;
    public bool buildingSelected;
    public GameObject requestIndicatorInstance;
    public GameObject driveTowardsIndicatorInstance;
    
    //Internal streetfinder detection
    [HideInInspector] public Street referencedStreet;
    private Vector3 _streetFinderStepDirection;
    private Vector3 _streetFinderRayOrigin;
    private RaycastHit _streetFinder;
    
    // Start is called before the first frame update
    void Awake()
    {
        buildingId = BuildingManager.GetNewGetBuildingId(this);
        
        transform.rotation = Quaternion.Euler(new Vector3(0, (int)orientation, 0));
        
        //Gather info on the street that the building is facing
        switch (orientation)
        {
            case BuildingOrientation.Bottom:
                _streetFinderStepDirection = Vector3.right;
                break;
            case BuildingOrientation.Top:
                _streetFinderStepDirection = Vector3.left;
                break;
            case BuildingOrientation.Right:
                _streetFinderStepDirection = Vector3.forward;
                break;
            case BuildingOrientation.Left:
                _streetFinderStepDirection = Vector3.back;
                break;
        }

        
        _streetFinderRayOrigin = new Vector3(transform.position.x, 50, transform.position.z);
        do
        {
            if (Vector3.Distance(new Vector3(transform.position.x, _streetFinderRayOrigin.y, transform.position.z), _streetFinderRayOrigin) > 100) //Avoid infinite loop
            {
                Debug.LogError("ERROR! Did not find any street in proximity of building. Skipping assignment - other errors may occur.");
                break;
            }
            
            if (Physics.Raycast(_streetFinderRayOrigin, Vector3.down, out _streetFinder))
            {
                if (_streetFinder.transform)
                {
                    if (_streetFinder.transform.GetComponent<Street>())
                        referencedStreet = _streetFinder.transform.GetComponent<Street>();
                }
            }

            _streetFinderRayOrigin += _streetFinderStepDirection;

        } while (referencedStreet == null);
    }

    public void RequestTaxi()
    {
        buildingSelected = true;
        requestingTaxi = true;
        buildingIdToDriveTowards = BuildingManager.GetPassengerDestinationBuildingId(buildingId);
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
