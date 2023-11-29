using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalJump: MonoBehaviour
    {
        [SerializeField] float m_Force = 20.0f;
        PlayerController m_PlayerController;

        private AudioSource m_AudioSource;

        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        void AddPortalJumpForce()
        {
            m_PlayerController.AddJumpForce(m_Force);
            m_AudioSource.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_PlayerController = other.GetComponent<PlayerController>();
                AddPortalJumpForce();
            }
        }
    }

}
