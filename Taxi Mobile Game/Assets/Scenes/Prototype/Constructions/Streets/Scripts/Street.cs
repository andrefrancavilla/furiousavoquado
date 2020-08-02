﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Street : MonoBehaviour
{
    public GameObject testVehicle;
    private float _laneStep; //Distance from center of street to center of right or left lane
    private GameObject _vehicleInstance;

    private void Start()
    {
    }

    public void PlaceVehicleRelativeToBuilding(GameObject vehiclePrefab, Vector3 buildingCoordinates)
    {
        StreetLane lane = transform.InverseTransformPoint(buildingCoordinates).x > 0 ? StreetLane.Right : StreetLane.Left;
        
        _laneStep = transform.localScale.x / 8;
        _vehicleInstance = Instantiate(vehiclePrefab, transform.TransformPoint(new Vector3((int) lane * _laneStep, 0)), transform.localRotation);
        
        if (Math.Abs(transform.rotation.y) > 0.1f)
        {
            _vehicleInstance.transform.position = new Vector3(buildingCoordinates.x, _vehicleInstance.transform.position.y, _vehicleInstance.transform.position.z);
        }
        else
        {
            _vehicleInstance.transform.position = new Vector3(_vehicleInstance.transform.position.x, _vehicleInstance.transform.position.y, buildingCoordinates.z);
        }
    }
}

public enum StreetLane
{
    Left = -1,
    Right = 1
}
