using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalUnlockUniverse : MonoBehaviour
    {
        [SerializeField] GameObject m_Area;
        [SerializeField] bool m_IsActive;

        [SerializeField] AudioClip m_ButtonSound;
        [SerializeField] AudioClip m_UnlockSound;

        private PlayerController m_PlayerController;
        private AudioSource m_AudioSource;

        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
        }

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

            m_AudioSource.PlayOneShot(m_ButtonSound);
            m_PlayerController.transform.forward = new Vector3(0, 0, 1);
            yield return new WaitForSeconds(0.5f);

            var bigBangs = m_Area.GetComponentsInChildren<BigBang>();
            int bigBangsActive = 0;

            foreach (var bigBang in bigBangs)
            {
                if (bigBang.GetComponent<Collider>().enabled) bigBangsActive++;
                bigBang.Active(true);
            }

            if (bigBangs.Length != bigBangsActive)
                m_AudioSource.PlayOneShot(m_UnlockSound);

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