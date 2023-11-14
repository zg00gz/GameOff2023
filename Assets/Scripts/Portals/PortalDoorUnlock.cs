using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalDoorUnlock : MonoBehaviour
    {
        [SerializeField] PortalDoor m_Door;
        [SerializeField] int m_KeysRequired;
        [SerializeField] bool m_IsTop;

        [SerializeField] MeshRenderer m_Button;
        [SerializeField] Material m_LockMaterial;
        [SerializeField] Material m_UnlockMaterial;

        [SerializeField] bool m_IsActive;

        PlayerController m_PlayerController;

        void Update()
        {
            if (m_IsActive && m_PlayerController.IsJumping == false)
            {
                Vector3 action = PlayerInput.Instance.Move;

                // Pour désactiver le déclenchement si le joueur appuit sur la direction inverse
                if (!m_IsTop && action.y < 0)
                    return;
                if (m_IsTop && action.y > 0)
                    return;

                if (PlayerInput.Instance.Action)
                {
                    StartCoroutine(TryToUnlock());
                    return;
                }
            }
        }

        IEnumerator TryToUnlock()
        {
            m_IsActive = false;
            m_PlayerController.SetKinematic(true);
            PlayerInput.Instance.ActionInput(false);

            m_PlayerController.transform.forward = new Vector3(0, 0, m_IsTop ? 1 : -1);
            yield return new WaitForSeconds(0.5f);

            // if(m_KeysRequired == m_PlayerController.Keys) OU Stars minimum
            m_Door.SetUnlocked();
            m_Button.material = m_UnlockMaterial;

            m_PlayerController.SetKinematic(false);
            this.enabled = false; // Désactive le collider
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
                m_IsActive = false;
            }
        }
    }
}