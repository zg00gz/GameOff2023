using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalDoorKey : MonoBehaviour
    {
        [SerializeField] ParticleSystem m_ParticleSystem;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                m_ParticleSystem.Play();
                GameManager.Instance.GetKey();
                Destroy(gameObject);
            }
        }
    }

}

