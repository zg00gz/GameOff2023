using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaleTravel
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleTrigger : MonoBehaviour
    {
        public bool m_PlayOnAwake;
        
        ParticleSystem m_ParticleSystem;

        private void Awake()
        {
            m_ParticleSystem = GetComponent<ParticleSystem>();

            if (m_PlayOnAwake) m_ParticleSystem.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            m_ParticleSystem.Play();
        }

        private void OnTriggerExit(Collider other)
        {
            m_ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}


