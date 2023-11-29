using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalTeleportBlackHole : MonoBehaviour
    {

        public bool IsIgnoringFirstTrigger;
        [SerializeField] PortalTeleportBlackHole m_PortalDestination;
        [SerializeField] GameObject m_Display;
        [SerializeField] GameObject m_Hide;

        GameObject m_Target;
        private AudioSource m_AudioSource;

        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

        void Teleport()
        {
            m_Target.GetComponent<PlayerController>().SetPosition(m_PortalDestination.transform.position);
            if (m_Display != null) m_Display.SetActive(true);
            if (m_Hide != null) m_Hide.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                if (IsIgnoringFirstTrigger)
                {
                    IsIgnoringFirstTrigger = false;
                    m_AudioSource.Play();
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
