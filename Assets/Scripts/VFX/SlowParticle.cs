using System;
using UnityEngine;

namespace VFX
{
    public class SlowParticle : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;

       private ParticleSystem[] _particleSystems;

       private void Awake()
       {
           _particleSystems = GetComponentsInChildren<ParticleSystem>();
       }

       void Start()
        {
            foreach (var particleSystem  in _particleSystems)
            {
                ParticleSystem.MainModule mainModule = particleSystem.main;
                mainModule.simulationSpeed = _speed;
            }
          
        }
    }
}