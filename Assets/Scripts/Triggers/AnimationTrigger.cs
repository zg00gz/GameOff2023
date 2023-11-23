using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaleTravel
{
    [RequireComponent(typeof(Animator))]
    public class AnimationTrigger : MonoBehaviour
    {
        [SerializeField] string m_TriggerStart;
        [SerializeField] string m_TriggerStop;

        Animator m_Animator;

        private void Awake()
        {
            m_Animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_Animator.SetTrigger(m_TriggerStart);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_Animator.SetTrigger(m_TriggerStop);
            }
        }
    }
}


