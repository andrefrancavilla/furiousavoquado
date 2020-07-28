using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapAndDrag : MonoBehaviour
{
    [Header("Configuration")]
    public GameObject tapParticle;
    public GameObject dragParticle;
    public float tapDeadZone;

    //Internal "pool"
    private GameObject _tapParticleInstance;
    private GameObject _dragParticleInstance;

    //Internal utility
    private Camera _mainCamera;
    private Vector2 _lastKnownFingerPosition;
    private Vector2 _currentWorldFingerPosition;
    private float _fingerTravelDistance;

    private void Start()
    {
        //Set local variables to reference them later. Avoids instantiating particles the time and keeps inspector tidy.
        _tapParticleInstance = Instantiate(tapParticle);
        tapParticle.SetActive(false);

        _dragParticleInstance = Instantiate(dragParticle);
        _dragParticleInstance.SetActive(false);

        //Camera.main invocation is expensive when used in game loop - save camera as a variable instead
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            _currentWorldFingerPosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, _mainCamera.transform.position.z + 1));
            
            if(_lastKnownFingerPosition != Vector2.zero)
                _fingerTravelDistance += Vector2.Distance(Input.GetTouch(0).position, _lastKnownFingerPosition);
            
            if (_fingerTravelDistance > tapDeadZone)
            {
                if(Vector2.Distance(_dragParticleInstance.transform.position,  _currentWorldFingerPosition) > tapDeadZone) //To avoid drawing particle from where the player last lifted his finger from
                    _dragParticleInstance.SetActive(false);
                
                _dragParticleInstance.transform.position = _currentWorldFingerPosition;
                _dragParticleInstance.SetActive(true);
            }
            
            _lastKnownFingerPosition = Input.GetTouch(0).position;
        }
        else
        {
            if (_lastKnownFingerPosition != Vector2.zero) //If it's not zero, then a finger input has occurred
            {
                if (_fingerTravelDistance <= tapDeadZone)
                {
                    //Setting the particle instance to false and then to true again will make the particle trigger from the start without having to reference the particle system.
                    _tapParticleInstance.SetActive(false);
                    _tapParticleInstance.transform.position = _mainCamera.ScreenToWorldPoint(new Vector3(_lastKnownFingerPosition.x, _lastKnownFingerPosition.y, 10));
                    _tapParticleInstance.SetActive(true);
                }

                //Reset all to default
                _fingerTravelDistance = 0;
                _lastKnownFingerPosition = Vector2.zero;
            }
        }
    }
}
