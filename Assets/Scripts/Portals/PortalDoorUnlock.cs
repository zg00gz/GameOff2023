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
        [SerializeField] SpriteRenderer m_Key;
        [SerializeField] SpriteRenderer m_Key2;
        [SerializeField] Material m_LockMaterial;
        [SerializeField] Material m_UnlockMaterial;

        [SerializeField] bool m_IsActive;

        PlayerController m_PlayerController;

        void Update()
        {
            if (m_IsActive && m_PlayerController.IsJumping == false)
            {
                if (PlayerInput.Instance.Action)
                {
                    //Debug.Log("Action !");
                    StartCoroutine(TryToUnlock());
                }
                PlayerInput.Instance.ActionInput(false);
            }
        }

        IEnumerator TryToUnlock()
        {
            m_IsActive = false;
            m_PlayerController.SetKinematic(true);

            m_PlayerController.transform.forward = new Vector3(0, 0, m_IsTop ? 1 : -1);
            yield return new WaitForSeconds(0.5f);

            bool canOpen = true;
            if (m_KeysRequired > 0)
            {
                if(GameManager.Instance.OpenWithKeys(m_KeysRequired))
                {
                    m_Key.enabled = false;
                    if (m_KeysRequired > 1) m_Key2.enabled = false;
                }
                else
                    canOpen = false;
            }

            if(canOpen)
            {
                m_Door.SetUnlocked();
                m_Button.material = m_UnlockMaterial;

                this.enabled = false; // Désactive le collider
            }
            else
            {
                m_IsActive = true;
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