using UnityEngine;
using UnityEngine.Events;


namespace ScaleTravel
{
    public class DestroyTrigger : MonoBehaviour
    {
        [SerializeField] GameObject[] m_ObjectToDestroy;

        private bool m_IsEndTriggered;

        private void OnTriggerEnter(Collider other)
        {
            if (!m_IsEndTriggered && other.CompareTag("Player") && !other.isTrigger)
            {
                m_IsEndTriggered = true;
                foreach(var obj in m_ObjectToDestroy)
                {
                    Destroy(obj);
                }
            }
        }

    }
}