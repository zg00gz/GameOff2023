using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalUnlockUniverse : MonoBehaviour
    {
        [SerializeField] GameObject m_Area;

        [SerializeField] bool m_IsActive;

        PlayerController m_PlayerController;

        void Update()
        {
            if (m_IsActive && m_PlayerController.IsJumping == false)
            {
                if (PlayerInput.Instance.Action)
                {
                    StopAllCoroutines();
                    m_IsActive = false;
                    StartCoroutine(Unlock());
                }
                PlayerInput.Instance.ActionInput(false);
            }
        }

        IEnumerator Unlock()
        {
            m_PlayerController.SetKinematic(true);

            m_PlayerController.transform.forward = new Vector3(0, 0, 1);
            yield return new WaitForSeconds(0.5f);

            foreach(var bigBang in m_Area.GetComponentsInChildren<BigBang>())
            {
                bigBang.Active(true);
            }

            m_PlayerController.SetKinematic(false);
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