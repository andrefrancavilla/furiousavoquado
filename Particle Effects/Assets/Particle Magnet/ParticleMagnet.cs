using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMagnet : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The magnet's attraction force.")]
    [Range(0.01f, 0.1f)]
    public float attractionForce;
    
    [Header("Debug & Test")]
    [Tooltip("Clicking on this while in play mode will manually activate the magnet. Useful for testing.")]
    [SerializeField] private bool activate; //Made private but serialized, accessible from Inspector but not from other classes.
    [SerializeField] private Vector2 debugTargetLocation; //Made private but serialized, accessible from Inspector but not from other classes.
    [SerializeField] private float debugMagnetLifeDuration; //Made private but serialized, accessible from Inspector but not from other classes.
    [SerializeField] private float debugMagnetStartDelay; //Made private but serialized, accessible from Inspector but not from other classes.

    
    //Internal
    private List<ParticleSystem.Particle> _particlesInsideCollision = new List<ParticleSystem.Particle>();
    private ParticleSystem _particleSystem;
    private ParticleSystem.Particle _particle;
    private ParticleSystem.TriggerModule _triggerModule;
    //--Magnet activation, internal logic
    private bool _magnetActivated;
    private Vector3 _targetPosition;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        _triggerModule = _particleSystem.trigger;
    }

    private void Update()
    {
        if (!activate) return;
        
        Activate(debugMagnetStartDelay, debugTargetLocation, debugMagnetLifeDuration);
        activate = false;
    }
    
    public void Activate(float delay, Vector3 position, float flyTime)
    {
        StartCoroutine(ActivateMagnet());
        
        IEnumerator ActivateMagnet() //Use coroutine to enable and disable the magnet easily with yields
        {
            //Prepare particle system to be attracted by the magnet if it isn't already or is configured improperly
            if (!_triggerModule.enabled || _triggerModule.inside != ParticleSystemOverlapAction.Callback)
            {
                GameObject clone = new GameObject();
                clone.AddComponent<BoxCollider2D>().size = new Vector2(500, 500);
                clone.transform.parent = transform;
                
                _triggerModule.enabled = true;
                _triggerModule.inside = ParticleSystemOverlapAction.Callback;
                _triggerModule.SetCollider(0, clone.GetComponent<BoxCollider2D>());
            }
            
            yield return new WaitForSeconds(delay);
            _targetPosition = position; //Assign position to variable. Then gets referenced in OnParticletrigger
            _magnetActivated = true;
            yield return new WaitForSeconds(flyTime);
            _magnetActivated = false;
        }
    }

    private void OnParticleTrigger()
    {
        _particleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, _particlesInsideCollision);

        for (int i = 0; i < _particlesInsideCollision.Count; i++)
        {
            _particle = _particlesInsideCollision[i];
            if(_magnetActivated)
            {
                _particle.velocity = Vector2.Lerp(_particle.velocity, _targetPosition - _particle.position, attractionForce);
            }
            _particlesInsideCollision[i] = _particle;
        }
        
        _particleSystem.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, _particlesInsideCollision);
    }
}
