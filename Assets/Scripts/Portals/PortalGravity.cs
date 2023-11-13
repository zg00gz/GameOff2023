using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalGravity : MonoBehaviour
    {
        [SerializeField] Collider m_PortalArrivalCollider;
        PlayerController m_PlayerController;

        void SetGravity()
        {
            PlayerInput.Instance.playerControllerInputBlocked = true;
            m_PlayerController.SetGravityInverted();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_PlayerController = other.GetComponent<PlayerController>();
                SetGravity();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_PortalArrivalCollider.enabled = true;
            }
        }

    }

}
