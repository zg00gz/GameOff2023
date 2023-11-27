using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ScaleTravel
{
    public class PortalKeys : MonoBehaviour
    {
        [SerializeField] SpriteRenderer m_Key;
        [SerializeField] bool m_IsActive;

        [SerializeField] GameObject[] m_AreaKeys;
        [SerializeField] int[] m_TargetKeys;

        [SerializeField] int m_AreaIndex = 0;

        PlayerController m_PlayerController;

        void Update()
        {
            if (m_IsActive && m_PlayerController.IsJumping == false)
            {
                if (PlayerInput.Instance.Action)
                {
                    m_IsActive = false;
                    StopAllCoroutines();
                    StartCoroutine(DisplayAreaKeys());
                }
                PlayerInput.Instance.ActionInput(false);
            }
        }
        
        IEnumerator DisplayAreaKeys()
        {  
            m_PlayerController.SetKinematic(true);

            m_PlayerController.transform.forward = new Vector3(0, 0, 1);
            yield return new WaitForSeconds(0.5f);

            m_AreaKeys[m_AreaIndex].SetActive(true);
            m_Key.enabled = false;

            // On détruit la zone de clés précédente
            if (m_AreaIndex > 0)
                Destroy(m_AreaKeys[m_AreaIndex-1]);

            m_PlayerController.SetKinematic(false);

            if(m_AreaIndex < m_AreaKeys.Length - 1)
                StartCoroutine(WaitingForkeys());
        }

        IEnumerator WaitingForkeys()
        {
            yield return new WaitUntil(() => GameManager.Instance.GetNumberOfKey() == m_TargetKeys[m_AreaIndex]);

            m_Key.enabled = true;
            m_AreaIndex++;
        }
        

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                m_IsActive = true;
                PlayerInput.Instance.ActionInput(false);
                m_PlayerController = other.GetComponent<PlayerController>();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                m_IsActive = false;
                PlayerInput.Instance.ActionInput(false);
            }
        }
    }
}