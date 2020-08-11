using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossroad : MonoBehaviour
{
    //Directions are *all* relative to the crossroad's forward (Z) axis
    public bool backwards; // -1
    public bool forwards; // 0
    public bool left; // 1
    public bool right; // 2
    public List<int> possibleDirections = new List<int>();

    public Vector3 relativePosition;
    private Vector3 _vehichleNewRot;
    private int _rndDir;

    void Start()
    {
        //Gather possible directions
        if (backwards)
            possibleDirections.Add(-1);
        if (forwards)
            possibleDirections.Add(0);
        if (left)
            possibleDirections.Add(1);
        if (right)
            possibleDirections.Add(2);
    }

    public Vector3 GetRotation(Vector3 vehiclePosition, Vector3 vehicleEulerAngles)
    {
        _vehichleNewRot = vehicleEulerAngles;
        relativePosition = transform.InverseTransformPoint(vehiclePosition);
        
        if (Mathf.Abs(relativePosition.x) > Mathf.Abs(relativePosition.z))
        {
            //Vehicle is coming from Right or Left
            if (relativePosition.x > 0) //Right
            {
                do
                {
                    _rndDir = possibleDirections[Random.Range(0, possibleDirections.Count)];
                } while (_rndDir == 2);

                switch (_rndDir)
                {
                    case  -1:
                        //Left for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y - 90, vehicleEulerAngles.z);
                        break;
                    case 0:
                        //Right for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 90, vehicleEulerAngles.z);
                        break;
                    case 1:
                        //Straight for vehicle
                        //Do nothing
                        break;
                    case 2:
                        //U turn for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 180, vehicleEulerAngles.z);
                        break;
                }
            }
            else //Left
            {
                do
                {
                    _rndDir = possibleDirections[Random.Range(0, possibleDirections.Count)];
                } while (_rndDir == 1);

                switch (_rndDir)
                {
                    case  -1:
                        //Right for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 90, vehicleEulerAngles.z);
                        break;
                    case 0:
                        //Left for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y - 90, vehicleEulerAngles.z);
                        break;
                    case 1:
                        //U turn for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 180, vehicleEulerAngles.z);
                        break;
                    case 2:
                        //Straight for vehicle
                        //Do nothing
                        break;
                }
            }
        }
        else
        {
            //Vehicle is coming from behind or up front
            if (relativePosition.z > 0)
            {
                do
                {
                    _rndDir = possibleDirections[Random.Range(0, possibleDirections.Count)];
                } while (_rndDir == 0);
                //Up front
                switch (_rndDir)
                {
                    case -1:
                        //Straight for vehicle
                        //Do nothing
                        break;
                    case 0:
                        //U turn for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 180, vehicleEulerAngles.z);
                        break;
                    case 1:
                        //Right for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 90, vehicleEulerAngles.z);
                        break;
                    case 2:
                        //Left for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y - 90, vehicleEulerAngles.z);
                        break;
                }
            }
            else
            {
                do
                {
                    _rndDir = possibleDirections[Random.Range(0, possibleDirections.Count)];
                } while (_rndDir == -1);
                //Behind
                switch (_rndDir)
                {
                    case  -1:
                        //U turn for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 180, vehicleEulerAngles.z);
                        break;
                    case 0:
                        //Straight for vehicle
                        //Do nothing
                        break;
                    case 1:
                        //Left for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y - 90, vehicleEulerAngles.z);
                        break;
                    case 2:
                        //Right for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 90, vehicleEulerAngles.z);
                        break;
                }
            }
        }

        return _vehichleNewRot;
    }
}
