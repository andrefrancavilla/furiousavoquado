using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    [Header("Gameplay Assets")]
    public GameObject needTaxiIndicator;
    public GameObject mustDriveHereIndicator;
    public GameObject taxiPrefab;
    public GameObject passengerPrefab;

    [Header("Player Stats")] 
    public int credits;
    
    [Header("Readonly")]
    public GUI gui;
    public CameraController camController;
    public List<Building> currentlyActiveRequests;
    public bool isSelectingBuilding;
    public int requestsRemaining; //The amount of requests before ending the game

    private Building _destinationBuilding;
    private Building _originBuilding;
    private Passenger _passengerInstance;
    private GameObject _taxiInstance;
    
    // Start is called before the first frame update
    void Start()
    {
        gui = FindObjectOfType<GUI>();
        camController = FindObjectOfType<CameraController>();
        requestsRemaining = Random.Range(12, 24);
        StartCoroutine(StartSessionRequests());
    }

    #region Taxi and Passenger
    public void PickUpPassenger(Building fromBuilding) //Hides all requests from screen, spawns taxi and passenger relative to the fromBuilding's position
    {
        for (int i = 0; i < currentlyActiveRequests.Count; i++)
        {
            if (currentlyActiveRequests[i] == fromBuilding)
            {
                currentlyActiveRequests[i].buildingSelected = true;
                
                _originBuilding = currentlyActiveRequests[i];
                _destinationBuilding = BuildingManager.GetBuildingById(currentlyActiveRequests[i].buildingIdToDriveTowards);
            }
            
            //Hide all taxi request indicators
            currentlyActiveRequests[i].requestIndicatorInstance.SetActive(false);
        }
        
        //Spawn taxi and passenger
        _taxiInstance = fromBuilding.referencedStreet.PlaceVehicleRelativeToBuilding(taxiPrefab, _originBuilding.transform.position);
        _taxiInstance.GetComponent<Taxi>().callingPassenger = true;

        _passengerInstance = Instantiate(passengerPrefab, _originBuilding.pedestrianSpawnPosition.position, _originBuilding.pedestrianSpawnPosition.rotation).GetComponent<Passenger>();
        
        isSelectingBuilding = true;
    }

    public void CallPassenger(Transform moveTarget) //Makes passenger walk towards taxi
    {
        _taxiInstance.GetComponent<Taxi>().callingPassenger = true;
        _passengerInstance.WalkTowards(moveTarget, _taxiInstance.GetComponent<Taxi>());
    }

    public void SetPassengerDestination() //When passenger enters the taxi, the player will know where to bring him
    {
        _destinationBuilding.driveTowardsIndicatorInstance = Instantiate(mustDriveHereIndicator, _destinationBuilding.requestIconPosition.position, transform.rotation, gui.transform);
        _destinationBuilding.mustDriveTowards = true;
    }

    public void DropOffPassenger(Transform vehicle) //Make passenger get out of the car
    {
        _passengerInstance = Instantiate(passengerPrefab, vehicle.position, vehicle.rotation).GetComponent<Passenger>();
        _passengerInstance.WalkTowards(_destinationBuilding.pedestrianSpawnPosition, _taxiInstance.GetComponent<Taxi>());
        RewardCredits(Random.Range(2, 5));
    }

    public void EndPassengerRequest(Building toBuilding) //Resets the UI to its original state, showing all passenger requests
    {
        Destroy(_originBuilding.requestIndicatorInstance);
        Destroy(_destinationBuilding.driveTowardsIndicatorInstance);
        
        _originBuilding.buildingSelected = false;
        _destinationBuilding.mustDriveTowards = false;
        
        _taxiInstance.GetComponent<Taxi>().FadeAway(); //Fade taxi that is in _originBuilding position
        _taxiInstance = toBuilding.referencedStreet.PlaceVehicleRelativeToBuilding(taxiPrefab, _destinationBuilding.transform.position); //Place taxi at new building
        
        currentlyActiveRequests.Remove(_originBuilding);
        
        /*for (int i = 0; i < currentlyActiveRequests.Count; i++)
        {
            currentlyActiveRequests[i].requestIndicatorInstance.SetActive(true);
        }*/
    }

    public void ShowRequests()
    {
        for (int i = 0; i < currentlyActiveRequests.Count; i++)
        {
            currentlyActiveRequests[i].requestIndicatorInstance.SetActive(true);
        }

        isSelectingBuilding = false;
    }
    
    #endregion
    
    #region Request Management

    private IEnumerator StartSessionRequests()
    {
        int randomBuildingId = 0;
        Building buildingIteration;
        do
        {
            do
            {
                randomBuildingId = Random.Range(1, BuildingManager.allBuildings.Count);
            } while (currentlyActiveRequests.Contains(BuildingManager.GetBuildingById(randomBuildingId)));

            buildingIteration = BuildingManager.GetBuildingById(randomBuildingId);
            
            buildingIteration.RequestTaxi();
            
            yield return new WaitForSeconds(Random.Range(4, 9));
            requestsRemaining--;
        } while (requestsRemaining > 0);
        
        Debug.Log("All buildings assigned!");
    }
    
    public void AddActiveRequest(Building requestingBuilding, Vector3 indicatorPosition)
    {
        currentlyActiveRequests.Add(requestingBuilding);
        requestingBuilding.requestIndicatorInstance = Instantiate(needTaxiIndicator, indicatorPosition, transform.rotation, gui.transform);
        requestingBuilding.requestIndicatorInstance.SetActive(!isSelectingBuilding); //Must not show if a building is already selected
    }
    #endregion

    #region Currency Management

    public void RewardCredits(int amount)
    {
        credits += amount;
        gui.UpdateCredits(credits);
    }

    #endregion'
}
