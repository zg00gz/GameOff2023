using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalTeleport : MonoBehaviour
    {

        public bool IsIgnoringFirstTrigger;
        [SerializeField] PortalTeleport m_PortalDestination;
        private AudioSource m_AudioSource;

        GameObject m_Target;

        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        void Teleport()
        {
            m_AudioSource.Play();
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
                }
            }
        }

    }

}
