using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour
{
    public float moveSpeed;
    public float steerSpeed;
    private Vector3 _localMovement;    
    private Vector3 _globalMovement;
    private Crossroad _currentCrossroad;
    private int _dirToSteerTowards; //0 = straight, 1 = left, 2 = right
    private bool _foundNewDirection;
    private Vector3 _targetRotation;

    private void Start()
    {
        _targetRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        _localMovement = new Vector3(0, 0, moveSpeed);
        _globalMovement = transform.TransformDirection(_localMovement);

        transform.position += _globalMovement * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_targetRotation), steerSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        _currentCrossroad = other.GetComponent<Crossroad>();
        if (_currentCrossroad)
        {
            do
            {
                _dirToSteerTowards = Random.Range(0, 3); //with integers max is exclusive
                switch (_dirToSteerTowards)
                {
                    case 0:
                        if (_currentCrossroad.forwards)
                        {
                            //Do not change rotation
                            _foundNewDirection = true;
                        }
                        break;
                    case 1:
                        if (_currentCrossroad.left)
                        {
                            _targetRotation = new Vector3(transform.eulerAngles.x, _currentCrossroad.transform.eulerAngles.y - 90);
                            _foundNewDirection = true;
                        }
                        break;
                    case 2:
                        if (_currentCrossroad.right)
                        {
                            _targetRotation = new Vector3(transform.eulerAngles.x, _currentCrossroad.transform.eulerAngles.y + 90);
                            _foundNewDirection = true;
                        }
                        break;
                }
            } while (!_foundNewDirection);

        }
    }
}
