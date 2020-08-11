using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Vehicle : MonoBehaviour
{
    [Header("Configuration")]
    public float moveSpeed;
    public float steerSpeed;
    public float steerLeftDelay;
    [Header("Readonly")]
    public Vector3 targetRotation;
    public bool collided;
    
    //Internal
    private float _steerLeftDelayMem;
    private bool _canFadeAway;
    private Vector3 _localMovement;    
    private Vector3 _globalMovement;
    private Crossroad _currentCrossroad;
    private VehicleSpawner _spawner;
    private Animator _anim;

    private void Start()
    {
        targetRotation = transform.eulerAngles;
        _steerLeftDelayMem = steerLeftDelay;
        steerLeftDelay = 0;
        _anim = GetComponent<Animator>();
        StartCoroutine(AllowFadingAway());
    }

    void Update()
    {
        _localMovement = new Vector3(0, 0, moveSpeed);
        _globalMovement = transform.TransformDirection(_localMovement);

        transform.position += _globalMovement * Time.deltaTime;

        if (steerLeftDelay > 0)
            steerLeftDelay -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(steerLeftDelay <= 0)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), steerSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collided) return;
        
        if(other.gameObject.layer != LayerMask.NameToLayer("Street")) 
            collided = true;

        _currentCrossroad = other.GetComponent<Crossroad>();
        if (_currentCrossroad)
        {
            targetRotation = _currentCrossroad.GetRotation(transform.position, transform.eulerAngles);
            if (targetRotation.y < transform.localEulerAngles.y)
            {
                steerLeftDelay = _steerLeftDelayMem;
            }
        }
        else if (_canFadeAway)
        {
            _spawner = other.GetComponent<VehicleSpawner>();
            if (_spawner)
            {
                _anim.SetTrigger("FadeAway");
                _spawner.AddVehicleToCount();
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        collided = false;
    }

    private IEnumerator AllowFadingAway()
    {
        yield return new WaitForSeconds(0.1f);
        _canFadeAway = true;
    }

    public void DestroyVehicle()
    {
        Destroy(gameObject);
    }
}
