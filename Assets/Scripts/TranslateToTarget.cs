using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class TranslateToTarget : MonoBehaviour
    {
        [SerializeField] Transform m_Object;
        [SerializeField] Transform m_Target;
        [SerializeField] float m_Speed = 2.0f;
        [SerializeField] bool m_IsLooping;

        private Vector3 m_InitialPosition;
        private Vector3 m_MovePosition;
        private bool m_IsTranslating;
        private bool m_IsTriggered;

        void Awake()
        {
            m_InitialPosition = m_Object.position;
        }

        void Update()
        {
            if(m_IsTriggered && m_IsTranslating)
            {
                var step = m_Speed * Time.deltaTime;
                m_Object.position = Vector3.MoveTowards(m_Object.position, m_MovePosition, step);

                if (m_Object.position == m_MovePosition)
                {
                    m_IsTranslating = false;
                    if(m_IsLooping) SetMovePosition();
                }
            }
        }

        private void SetMovePosition()
        {
            var currentPosition = m_Object.position;

            if (currentPosition == m_InitialPosition)
                m_MovePosition = m_Target.position;
            else if (currentPosition == m_Target.position)
                m_MovePosition = m_InitialPosition;

            m_IsTranslating = true;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                m_IsTriggered = true;
                SetMovePosition();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                m_IsTriggered = false;
            }
        }
    }

}

