using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalJump: MonoBehaviour
    {
        [SerializeField] float m_Force = 20.0f;
        PlayerController m_PlayerController;

        void AddPortalJumpForce()
        {
            m_PlayerController.AddJumpForce(m_Force);
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
