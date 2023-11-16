using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScaleTravel
{

    public class EndTrigger : MonoBehaviour
    {
        public UnityEvent OnEnd = new UnityEvent();

        private bool m_IsEndTriggered;

        private void PlayEvent()
        {
            OnEnd.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_IsEndTriggered && other.CompareTag("Player") && other.isTrigger)
            {
                m_IsEndTriggered = true;
                GameManager.Instance.LevelDone();
                PlayEvent();
            }
        }

    }

}

