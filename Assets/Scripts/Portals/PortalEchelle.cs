using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalEchelle : MonoBehaviour
    {
        [SerializeField] bool m_IsActive;
        [SerializeField] bool m_IsActionStarted;

        PlayerController m_PlayerController;
        Rigidbody m_PlayerRigidbody;

        float m_Speed = 4.0f;
        float m_EchelleOffsetY = 5.0f; // Taille y de l'objet échelle
        float m_EchelleOffsetZ = 1.0f; // Positionnement x au centre de la plateforme
        int m_NextStep = 1;
        Vector3 m_Move1;
        Vector3 m_Move2;
        Vector3 m_Move3;

        void Awake()
        {
            // Calcul de la hauteur en fonction du scale de l'objet
            float positionY = transform.position.y + (m_EchelleOffsetY * transform.localScale.y);
            Debug.Log(positionY);

            m_Move1 = transform.position;
            m_Move2 = new Vector3(transform.position.x, positionY, transform.position.z);
            m_Move3 = new Vector3(transform.position.x, positionY, transform.position.z + m_EchelleOffsetZ);
        }


        /*
        void Update()
        {
            if (m_IsActive && !m_IsActionStarted)
            {
                Vector3 action = PlayerInput.Instance.Action;

                if (action.magnitude >= 0.75f)
                {
                    m_IsActionStarted = true;

                    m_PlayerController.SetKinematic(true);
                    m_PlayerController.transform.LookAt(transform);
                }
            }
        }

        
        void FixedUpdate()
        {
            if (m_IsActionStarted)
            {
                var step = m_Speed * Time.fixedDeltaTime;

                if (m_PlayerController.transform.position != m_Move1)
                {
                    //m_PlayerRigidbody.MovePosition(m_PlayerController.transform.position + m_Move1 * step);
                }
                else if (m_PlayerController.transform.position != m_Move2)
                {
                    //m_PlayerRigidbody.MovePosition((m_PlayerController.transform.position + m_Move2).normalized * step);
                }
                else if (m_PlayerController.transform.position != m_Move3)
                {
                    //m_PlayerRigidbody.MovePosition((m_PlayerController.transform.position + m_Move3).normalized * step);
                }
                else
                {
                    m_IsActive = false;
                    m_IsActionStarted = false;
                    m_PlayerController.SetKinematic(false);
                }
            }
        }
        */
        

        
        void Update()
        {
            if (m_IsActive && m_PlayerController.m_IsJumping == false)
            {
                if (m_IsActionStarted)
                {
                    var step = m_Speed * Time.deltaTime;

                    if (m_NextStep == 1)
                    {
                        Debug.Log("Step 1");
                        m_PlayerController.transform.position = Vector3.MoveTowards(m_PlayerController.transform.position, m_Move1, step);
                        if (m_PlayerController.transform.position == m_Move1) m_NextStep = 2;
                    }
                    else if (m_NextStep == 2)
                    {
                        Debug.Log("Step 2");

                        m_PlayerController.transform.position = Vector3.MoveTowards(m_PlayerController.transform.position, m_Move2, step);
                        //m_PlayerController.transform.Translate(Vector3.Normalize(m_Move2 - m_PlayerController.transform.position) * step);
                        //m_PlayerController.transform.position = m_Move2;
                        if (m_PlayerController.transform.position == m_Move2) m_NextStep = 3;
                    }
                    else if (m_NextStep == 3)
                    {
                        Debug.Log("Step 3");

                        m_PlayerController.transform.position = Vector3.MoveTowards(m_PlayerController.transform.position, m_Move3, step);
                        //m_PlayerController.transform.position = m_Move3;
                        if (m_PlayerController.transform.position == m_Move3)
                        {
                            m_IsActive = false;
                            m_IsActionStarted = false;
                            m_NextStep = 1;
                            m_PlayerController.SetKinematic(false);
                        }
                    }
                    return;
                }
                
                Vector3 action = PlayerInput.Instance.Action;

                if (!m_IsActionStarted && action.magnitude >= 0.75f)
                {
                    m_IsActionStarted = true;

                    m_PlayerController.SetKinematic(true);
                    m_PlayerController.transform.LookAt(transform);
                    return;
                }
            }
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_PlayerController = other.GetComponent<PlayerController>();
                m_PlayerRigidbody = PlayerController.Instance.m_Rigidbody;
                m_IsActive = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                //m_IsActive = false;
            }
        }
    }
}

