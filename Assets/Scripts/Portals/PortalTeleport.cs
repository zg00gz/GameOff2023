using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalTeleport : MonoBehaviour
    {

        [SerializeField] bool m_IsIgnoringFirstTrigger;
        [SerializeField] Transform m_PortalDestination;

        GameObject m_Target;

        void Teleport()
        {
            m_Target.GetComponent<PlayerController>().SetPosition(m_PortalDestination.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                if (m_IsIgnoringFirstTrigger)
                {
                    m_IsIgnoringFirstTrigger = false;
                }
                else
                {
                    m_Target = other.gameObject;
                    Teleport();
                    m_IsIgnoringFirstTrigger = true; // Pour le voyage retour
                }
            }
        }

    }

}
