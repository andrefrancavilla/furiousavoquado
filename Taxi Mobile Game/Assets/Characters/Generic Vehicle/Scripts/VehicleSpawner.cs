using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleSpawner : MonoBehaviour
{
    public GameObject[] vehicles;
    [Range(1, 10)] public float minimumWaitTime;
    [Range(1, 20)] public float maximumWaitTime;
    public int amountToSpawn;
    public Street referencedStreetToSpawnOn;

    // Update is called once per frame
    void Start()
    {
        StartCoroutine(StartSpawningVehicles());
    }

    private IEnumerator StartSpawningVehicles()
    {
        do
        {
            amountToSpawn--;
            referencedStreetToSpawnOn.PlaceVehicle(vehicles[Random.Range(0, vehicles.Length)], transform.position);
            yield return new WaitForSeconds(Random.Range(minimumWaitTime, maximumWaitTime));
        } while (amountToSpawn > 0);
    }
}
