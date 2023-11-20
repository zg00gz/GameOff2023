using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalEchelleFall : MonoBehaviour
    {
        private Animator m_Animator;
        private bool m_IsFallen;

        void Awake()
        {
            m_Animator = GetComponent<Animator>();    
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!m_IsFallen && collision.gameObject.CompareTag("Player"))
            {
                m_IsFallen = true;
                m_Animator.SetTrigger("fall");
            }
        }
    }

}

