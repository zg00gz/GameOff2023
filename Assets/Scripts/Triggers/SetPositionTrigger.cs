using UnityEngine;
using UnityEngine.Events;


namespace ScaleTravel
{
    public class SetPositionTrigger : MonoBehaviour
    {
        [SerializeField] Transform m_Target;
        [SerializeField] Vector3 m_Position;

        public UnityEvent OnStart= new UnityEvent();

        [SerializeField] bool m_IsOneShot;
        private bool m_IsEndTriggered;

        private void SetPosition()
        {
            m_Target.position = m_Position;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_IsEndTriggered && other.CompareTag("Player") && !other.isTrigger)
            {
                if(m_IsOneShot) m_IsEndTriggered = true;
                SetPosition();
                OnStart.Invoke();
            }
        }

    }
}