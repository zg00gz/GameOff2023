using UnityEngine;


namespace ScaleTravel
{
    public class BigBang : MonoBehaviour
    {
        [SerializeField] GameObject m_Universe;
        [SerializeField] ParticleSystem m_ParticleExplosion;
        [SerializeField] AudioClip m_ExplosionSound;


        [SerializeField] float m_Radius = 5.0f;
        [SerializeField] float m_Force = 10.0f;

        public void Active(bool value)
        {
            GetComponent<Collider>().enabled = value;
            m_Universe.SetActive(value);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !other.isTrigger)
            {
                Active(false);

                GetComponent<AudioSource>().PlayOneShot(m_ExplosionSound);
                m_ParticleExplosion.Play();
                PlayerController.Instance.AddExplosionForce(m_Force, transform.position, m_Radius);
            }
        }

    }
}

