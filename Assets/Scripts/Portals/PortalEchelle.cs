using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalEchelle : MonoBehaviour
    {
        [SerializeField] bool m_IsActive;
        [SerializeField] bool m_IsActionStarted;
        [SerializeField] bool m_IsEchelleFront;
        [SerializeField] bool m_IsEchelleInverted;
        [SerializeField] bool m_IsTopCollider;


        public bool IsStartingPoint;
        [SerializeField] Transform m_Move1; // D�part de l'�chellle
        [SerializeField] Transform m_Move2; // Arriv�e �chelle
        [SerializeField] Transform m_Move3; // Arriv�e
        [SerializeField] float m_ScaleRequired = 1.0f;

        int m_NextStep = 1;

        PlayerController m_PlayerController;
        Vector3 m_PlayerStartPosition;
        float m_Speed = 4.0f;
        void Awake()
        {
            // TODO repositionner z du Collider si scale -- ? � tester 
        }


        // TODO si gestion Move dans Player controller => enlever Update et fonctionner avec des Coroutines => wait  position == ... 
        void Update()
        {
            if (m_IsActive && m_PlayerController.IsJumping == false)
            {
                if (m_IsActionStarted)
                {
                    var step = m_Speed * Time.deltaTime;

                    switch(m_NextStep)
                    {
                        case 1:
                            //Debug.Log("Step 1");
                            // TODO d�placer MoveTowards+Speed et setKinematic dans une fonction dans PlayerController
                            m_PlayerController.transform.position = Vector3.MoveTowards(m_PlayerController.transform.position, m_Move1.position, step);
                            if (m_PlayerController.transform.position == m_Move1.position)
                            {
                                m_NextStep = 2;
                                // TODO Position de dos en descente => animation ?
                                //if (m_IsTopCollider) m_PlayerController.transform.LookAt(m_Move1);
                                if (m_IsTopCollider) m_PlayerController.transform.forward = m_IsEchelleFront ? new Vector3(0, 0, m_Move1.position.z) : new Vector3(m_Move1.position.x, 0, 0);
                            }
                            break;
                        case 2:
                            if(m_ScaleRequired < m_PlayerController.transform.localScale.x)
                            {
                                //Debug.Log("Step 2 go back...");
                                // Retour en arri�re => trop petit pour monter !
                                m_PlayerController.transform.forward = m_PlayerStartPosition;
                                m_PlayerController.transform.position = Vector3.MoveTowards(m_PlayerController.transform.position, m_PlayerStartPosition, step);
                                if (m_PlayerController.transform.position == m_PlayerStartPosition)
                                {
                                    m_NextStep = 10;
                                }
                            }
                            else
                            {
                                //Debug.Log("Step 2");
                                m_PlayerController.transform.position = Vector3.MoveTowards(m_PlayerController.transform.position, m_Move2.position, step);
                                if (m_PlayerController.transform.position == m_Move2.position)
                                {
                                    m_NextStep = 3;
                                    if (m_IsTopCollider) m_PlayerController.transform.forward = m_IsEchelleFront ? new Vector3(0, 0, -m_Move1.position.z) : new Vector3(-m_Move1.position.x, 0, 0);
                                }
                            }
                            break;
                        case 3:
                            //Debug.Log("Step 3");
                            m_PlayerController.transform.position = Vector3.MoveTowards(m_PlayerController.transform.position, m_Move3.position, step);
                            if (m_PlayerController.transform.position == m_Move3.position)
                            {
                                m_IsActive = false;
                                m_IsActionStarted = false;
                                m_NextStep = 1;
                                m_PlayerController.SetKinematic(false);
                            }
                            break;
                    }
                    return;
                }
                
                Vector3 action = m_IsEchelleFront ? PlayerInput.Instance.MoveVertical : PlayerInput.Instance.Move;

                if (!m_IsEchelleFront)
                {
                    // Pour permettre au joueur de sortir du trigger d'arriv�e
                    float invert = m_IsEchelleInverted ? -1.0f : 1.0f;

                    if (m_IsTopCollider && invert * action.x > 0)
                        return;
                    if (!m_IsTopCollider && invert * action.x < 0)
                        return;
                }
                else
                {
                    // Pour d�sactiver le d�clenchement si le joueur appuit sur la direction inverse
                    if (!m_IsTopCollider && action.y < 0)
                        return;
                    if (m_IsTopCollider && action.y > 0)
                        return;
                }

                if (IsStartingPoint && !m_IsActionStarted && action.magnitude >= 0.85f)
                {
                    m_IsActionStarted = true;
                    IsStartingPoint = false;
                    m_PlayerStartPosition = m_PlayerController.transform.position;

                    m_PlayerController.SetKinematic(true);
                    m_PlayerController.transform.LookAt(m_Move1);
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

                if (!IsStartingPoint) IsStartingPoint = true;
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

