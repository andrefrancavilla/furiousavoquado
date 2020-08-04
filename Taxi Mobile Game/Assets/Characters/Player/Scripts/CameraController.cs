using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Configuration")]
    public float tapDeadZone;
    public float moveSensitivity;
    public float zoomSensitivity;

    //Internal utility
    //Panning
    public Camera mainCamera;
    private Vector2 _lastKnownFingerPosition;
    private Vector2 _currentWorldFingerPosition;
    private Vector3 _localCameraMovement;
    
    //Tap detection
    private float _fingerTravelDistance;
    private RaycastHit _hit;

    //Pinch
    private Vector2 _finger1Position;
    private Vector2 _finger2Position;
    private float _previousDistanceBetweenFingers;
    private float _currentDistanceBetweenFingers;
    private bool _zooming;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1 && !_zooming)
            {
                //Panning (if not tap selecting)
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
                if (Input.touchCount >= 2)
                {
                    //Pinch zoom
                    _finger1Position = new Vector2(Input.GetTouch(0).position.x / Screen.currentResolution.width, Input.GetTouch(0).position.y / Screen.currentResolution.height);
                    _finger2Position = new Vector2(Input.GetTouch(1).position.x / Screen.currentResolution.width, Input.GetTouch(1).position.y / Screen.currentResolution.height);
                    _currentDistanceBetweenFingers = Vector2.Distance(_finger1Position, _finger2Position);
                    if (_previousDistanceBetweenFingers != _currentDistanceBetweenFingers)
                    {
                        if (_previousDistanceBetweenFingers > 0)
                        {
                            mainCamera.orthographicSize -= (_currentDistanceBetweenFingers - _previousDistanceBetweenFingers) * zoomSensitivity;
                            _zooming = true;
                        }
                        _previousDistanceBetweenFingers = _currentDistanceBetweenFingers;
                    }
                }
            }
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
            
            //Reset pinch zoom
            _finger1Position = Vector2.zero;
            _finger2Position = Vector2.zero;
            _currentDistanceBetweenFingers = 0;
            _previousDistanceBetweenFingers = 0;
            _zooming = false;
        }
    }
}
