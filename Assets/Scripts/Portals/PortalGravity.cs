using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalGravity : MonoBehaviour
    {
        [SerializeField] Collider m_PortalArrivalCollider;
        PlayerController m_PlayerController;
        //GameObject m_Target;
        
        // TODO update si active et velocity x = 0 et m_isJumping = false

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
                //m_Target = other.gameObject;
                SetGravity(); // TODO active true
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                // Todo active false
                m_PortalArrivalCollider.enabled = true;
            }
        }

    }

}
