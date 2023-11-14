using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalTeleport : MonoBehaviour
    {

        public bool IsIgnoringFirstTrigger;
        [SerializeField] PortalTeleport m_PortalDestination;

        GameObject m_Target;

        void Teleport()
        {
            m_Target.GetComponent<PlayerController>().SetPosition(m_PortalDestination.transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                if (IsIgnoringFirstTrigger)
                {
                    IsIgnoringFirstTrigger = false;
                }
                else
                {
                    m_Target = other.gameObject;
                    m_PortalDestination.IsIgnoringFirstTrigger = true;
                    Teleport();
                    //m_IsIgnoringFirstTrigger = true; // Pour le voyage retour
                }
            }
        }

    }

}
