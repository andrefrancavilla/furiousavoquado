using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour
{
    public float moveSpeed;
    public float steerSpeed;
    public Vector3 targetRotation;
    private Vector3 _localMovement;    
    private Vector3 _globalMovement;
    private Crossroad _currentCrossroad;
    private bool _collided;

    private void Start()
    {
        targetRotation = transform.eulerAngles;
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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), steerSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collided) return;
        _collided = true;

        _currentCrossroad = other.GetComponent<Crossroad>();
        if (_currentCrossroad)
        {
            targetRotation = _currentCrossroad.GetRotation(transform.position, transform.eulerAngles);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        _collided = false;
    }
}
