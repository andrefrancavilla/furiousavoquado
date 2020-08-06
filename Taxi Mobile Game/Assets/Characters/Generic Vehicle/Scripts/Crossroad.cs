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

    private Vector3 _relativePosition;
    private Vector3 _vehichleNewRot;
    private List<int> _possibleDirections = new List<int>();

    public Vector3 GetRotation(Vector3 vehiclePosition, Vector3 vehicleEulerAngles)
    {
        _possibleDirections.Clear();
        _vehichleNewRot = vehicleEulerAngles;
        _relativePosition = transform.TransformPoint(vehiclePosition);

        //Gather possible directions
        if(backwards)
            _possibleDirections.Add(-1);
        if(forwards)
            _possibleDirections.Add(0);
        if(left)
            _possibleDirections.Add(1);
        if(right)
            _possibleDirections.Add(2);
        
        if (Mathf.Abs(_relativePosition.x) > 0.75f)
        {
            //Vehicle is coming from above or below
            if (_relativePosition.x > 0) //Below
            {
                switch (_possibleDirections[Random.Range(0, _possibleDirections.Count)])
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
            else //above
            {
                switch (_possibleDirections[Random.Range(0, _possibleDirections.Count)])
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
                        //Straight for vehicle
                        //Do nothing
                        break;
                    case 2:
                        //U turn for vehicle
                        _vehichleNewRot = new Vector3(vehicleEulerAngles.x, vehicleEulerAngles.y + 180, vehicleEulerAngles.z);
                        break;
                }
            }
        }
        else
        {
            if (Mathf.Abs(_relativePosition.z) > 0.75f)
            {
                //Vehicle is coming from behind or up front
                if (_relativePosition.z > 0)
                {
                    //Up front
                }
                else
                {
                    //Behind
                    switch (_possibleDirections[Random.Range(0, _possibleDirections.Count)])
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
        }

        return _vehichleNewRot;
    }
}
