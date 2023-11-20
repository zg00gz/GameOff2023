using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class MoveToTarget : MonoBehaviour
    {
        [SerializeField] bool m_IsActive;
        [SerializeField] bool m_IsActionStarted;

        [SerializeField] Transform m_Target;
        [SerializeField] bool m_IsFront;
        [SerializeField] bool m_IsInverted;

        PlayerController m_PlayerController;
        float m_Speed = 4.0f;

        void Update()
        {
            if (m_IsActive && m_PlayerController.IsJumping == false)
            {
                if (m_IsActionStarted)
                {
                    var step = m_Speed * Time.deltaTime;

                    m_PlayerController.transform.position = Vector3.MoveTowards(m_PlayerController.transform.position, m_Target.position, step);
                    if (m_PlayerController.transform.position == m_Target.position)
                    {
                        m_PlayerController.transform.forward = m_IsFront ? new Vector3(0, 0, -m_Target.position.z) : new Vector3(-m_Target.position.x, 0, 0);
                    }
                    return;
                }
                
                Vector3 action = m_IsFront ? PlayerInput.Instance.MoveVertical : PlayerInput.Instance.Move;

                if (!m_IsFront)
                {
                    if (!m_IsInverted && action.x > 0)
                        return;
                    if (m_IsInverted && action.x < 0)
                        return;
                }
                else
                {
                    if (!m_IsInverted && action.y < 0)
                        return;
                    if (m_IsInverted && action.y > 0)
                        return;
                }

                if (!m_IsActionStarted && action.magnitude >= 0.85f)
                {
                    m_IsActionStarted = true;
                    m_PlayerController.SetKinematic(true);
                    m_PlayerController.transform.LookAt(m_Target);
                    return;
                }
            }
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_IsActive = true;
                m_PlayerController = other.GetComponent<PlayerController>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
               if(!m_IsActionStarted) m_IsActive = false;
            }
        }
    }
}

