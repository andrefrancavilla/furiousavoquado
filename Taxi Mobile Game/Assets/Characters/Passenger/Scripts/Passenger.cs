using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passenger : MonoBehaviour
{
    public float walkSpeed;
    public float distToFade; //When pedestrian is this number of units distant from target, he disappears
    
    private Transform _walkToTarget;
    private Taxi _taxi;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_walkToTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, _walkToTarget.position, walkSpeed);

            if (Vector3.Distance(transform.position, _walkToTarget.position) < distToFade)
            {
                //When passenger reaches destination
                if (_taxi.transform.position == _walkToTarget.position)
                    BuildingManager.gameplayManager.SetPassengerDestination();
                else
                {
                    _taxi.FadeAway();
                }
                Destroy(gameObject);
            }
        }
    }

    public void WalkTowards(Transform target, Taxi taxi)
    {
        _walkToTarget = target;
        
        if(taxi)
            _taxi = taxi;
    }
}
