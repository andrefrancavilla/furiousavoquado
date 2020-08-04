using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taxi : MonoBehaviour
{
    public bool callingPassenger;
    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void FadeAway() //Called by passenger when he reaches destination
    {
        _anim.SetTrigger("FadeAway");
        
        if(callingPassenger)
            BuildingManager.gameplayManager.SetPassengerDestination();
    }
    
    #region Animation Events
    public void HandlePassenger() //Animation Event called at the end of the FadeIn animation
    {
        if (callingPassenger)
        {
            BuildingManager.gameplayManager.CallPassenger(transform);
        }
        else
        {
            BuildingManager.gameplayManager.DropOffPassenger(transform);
        }
    }

    public void EndFade() //Animation event called after taxi fades away
    {
        Destroy(gameObject);
    }
    #endregion
}
