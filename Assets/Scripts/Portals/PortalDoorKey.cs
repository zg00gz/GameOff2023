using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalDoorKey : MonoBehaviour
    {
        [SerializeField] ParticleSystem m_ParticleSystem;
        [SerializeField] AudioClip m_KeySound;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                m_ParticleSystem.Play();
                m_ParticleSystem.GetComponent<AudioSource>().PlayOneShot(m_KeySound);
                GameManager.Instance.GetKey();
                Destroy(gameObject);
            }
        }
    }

}

