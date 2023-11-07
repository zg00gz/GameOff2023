using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalGravityController : MonoBehaviour
    {
        [SerializeField] bool m_IsActive;
        [SerializeField] float m_Delay;
        [SerializeField] float m_Duration;
        [SerializeField] bool m_IsGravityInverted;

        GameObject m_Target;
        bool m_IsActionRunning;

        void Start()
        {
            // TODO ajouter image perso inversé ou non. Suivant le type
        }

        void Update()
        {
            if (m_IsActive)
            {
                if (!m_IsActionRunning)
                {
                    if (m_Delay > 0)
                    {
                        // TODO si delay => coroutine + isactive false => à la fin de la coroutine m_Target = null
                    }
                    if (m_Duration > 0)
                    {
                        // A la fin de la durée la gravité s'inverse => !m_IsGravityInverted
                    }

                    m_Target.GetComponent<PlayerController>().SetGravityInverted(m_IsGravityInverted);

                    m_IsActionRunning = true;
                }
                else
                {
                    
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                m_Target = other.gameObject;
                m_IsActive = true;
            }
        }
    }

}
