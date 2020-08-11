using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Random = UnityEngine.Random;

public class VehicleSpawner : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject[] vehicles;
    [Range(1, 10)] public float minimumWaitTime;
    [Range(1, 20)] public float maximumWaitTime;
    public int amountToSpawn;
    public Street referencedStreetToSpawnOn;
    
    //Internal
    private float _currT;
    
    private void Update()
    {
        if(amountToSpawn > 0)
        {
            if (_currT <= 0)
            {
                amountToSpawn--;
                referencedStreetToSpawnOn.PlaceVehicle(vehicles[Random.Range(0, vehicles.Length)], transform.position);
                _currT = Random.Range(minimumWaitTime, maximumWaitTime);
            }
            else
            {
                _currT -= Time.deltaTime;
            }
        }
    }

    public void AddVehicleToCount()
    {
        amountToSpawn++;
    }
}
