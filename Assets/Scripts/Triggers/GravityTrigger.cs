using UnityEngine;
using UnityEngine.Events;


namespace ScaleTravel
{
    public class GravityTrigger : MonoBehaviour
    {
        [SerializeField] Vector3 m_NewGravity;

        public UnityEvent OnStart= new UnityEvent();

        private bool m_IsEndTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!m_IsEndTriggered && other.CompareTag("Player") && !other.isTrigger)
            {
                m_IsEndTriggered = true;
                Physics.gravity = m_NewGravity;
                OnStart.Invoke();
            }
        }

    }
}