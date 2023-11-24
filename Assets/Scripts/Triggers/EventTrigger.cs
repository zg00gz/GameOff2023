using UnityEngine;
using UnityEngine.Events;


namespace ScaleTravel
{
    public class EventTrigger : MonoBehaviour
    {
        public UnityEvent OnStart= new UnityEvent();

        private bool m_IsEndTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!m_IsEndTriggered && other.CompareTag("Player") && !other.isTrigger)
            {
                m_IsEndTriggered = true;
                OnStart.Invoke();
            }
        }

    }
}