using UnityEngine;
using UnityEngine.Events;


namespace ScaleTravel
{
    public class EventTrigger : MonoBehaviour
    {
        [SerializeField] bool m_IsAlwaysActive;

        public UnityEvent OnStart= new UnityEvent();

        public UnityEvent OnEnd= new UnityEvent();

        private bool m_IsEndTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!m_IsEndTriggered && other.CompareTag("Player") && !other.isTrigger)
            {
                if(!m_IsAlwaysActive) m_IsEndTriggered = true;
                OnStart.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!m_IsEndTriggered && other.CompareTag("Player") && !other.isTrigger)
            {
                if (!m_IsAlwaysActive) m_IsEndTriggered = true;
                OnEnd.Invoke();
            }
        }

    }
}