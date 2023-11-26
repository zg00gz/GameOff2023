using UnityEngine;
using UnityEngine.Events;


namespace ScaleTravel
{
    public class AddPositionTrigger : MonoBehaviour
    {
        [SerializeField] Transform m_Target;
        [SerializeField] Vector3 m_PositionToAdd;

        public UnityEvent OnStart= new UnityEvent();

        [SerializeField] bool m_IsOneShot;
        private bool m_IsEndTriggered;

        private void AddPosition()
        {
            m_Target.position += m_PositionToAdd;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!m_IsEndTriggered && other.CompareTag("Player") && !other.isTrigger)
            {
                if(m_IsOneShot) m_IsEndTriggered = true;
                AddPosition();
                OnStart.Invoke();
            }
        }

    }
}