using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Configuration")]
    public float tapDeadZone;
    public float moveSensitivity;

    //Internal utility
    public Camera mainCamera;
    private Vector2 _lastKnownFingerPosition;
    private Vector2 _currentWorldFingerPosition;
    private Vector3 _localCameraMovement;
    
    private float _fingerTravelDistance;
    private RaycastHit _hit;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            _currentWorldFingerPosition = Input.GetTouch(0).position;
            
            if(_lastKnownFingerPosition != Vector2.zero)
            {
                _fingerTravelDistance += Vector2.Distance(_currentWorldFingerPosition, _lastKnownFingerPosition);

                if (_fingerTravelDistance > tapDeadZone)
                {
                    _localCameraMovement = new Vector3((_currentWorldFingerPosition.x -_lastKnownFingerPosition.x) / Screen.currentResolution.width, 0, 
                        (_currentWorldFingerPosition.y - _lastKnownFingerPosition.y) / Screen.currentResolution.height) * moveSensitivity;
                    transform.position -= transform.TransformDirection(_localCameraMovement);
                }
            }
            _lastKnownFingerPosition = Input.GetTouch(0).position;
        }
        else
        {
            if (_lastKnownFingerPosition != Vector2.zero) //If it's not zero, then a finger input has occurred
            {
                if (_fingerTravelDistance <= tapDeadZone)
                {
                    //Attempt interaction
                    if (Physics.Raycast(mainCamera.ScreenToWorldPoint(_lastKnownFingerPosition), mainCamera.transform.forward, out _hit))
                    {
                        if (_hit.transform)
                        {
                            if (_hit.transform.CompareTag("Building"))
                            {
                                if (_hit.transform.GetComponent<Building>().requestingTaxi && !_hit.transform.GetComponent<Building>().mustDriveTowards)
                                {
                                    BuildingManager.gameplayManager.PickUpPassenger(_hit.transform.GetComponent<Building>());
                                }
                                else
                                {
                                    if (_hit.transform.GetComponent<Building>().mustDriveTowards)
                                    {
                                        BuildingManager.gameplayManager.EndPassengerRequest(_hit.transform.GetComponent<Building>());
                                    }
                                }
                            }
                        }
                    }
                }

                //Reset all to default
                _fingerTravelDistance = 0;
                _lastKnownFingerPosition = Vector2.zero;
            }
        }
    }
}
