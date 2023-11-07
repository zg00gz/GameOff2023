using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaleTravel
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float m_Speed = 2.0f;
        [SerializeField] float m_JumpHeight = 1f;
        private float m_GravityValue = -9.81f;

        private CharacterController m_Controller;
        private Vector3 m_Velocity;
        [SerializeField] bool m_IsGrounded = true;
        private bool m_IsGravityChanged;
        

        private void Start()
        {
            m_Controller = GetComponent<CharacterController>();
        }

        void Update()
        {
            m_IsGrounded = m_Controller.isGrounded;
            if (m_IsGrounded && m_Velocity.y < 0)
            {
                m_Velocity.y = 0f;
            }

            // Vitesse en fonction de la taille
            float ratioScaleSpeed = transform.localScale.x < 1 ? 1/transform.localScale.x : 1.0f;
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            m_Controller.Move(move * Time.deltaTime * m_Speed * ratioScaleSpeed);

            if (move != Vector3.zero)
            {
                //transform.up = m_GravityValue > 0 ? Vector3.down : Vector3.up;
                //if (m_GravityValue < 0)
                //    transform.forward = move;
                //else
                //    transform.right = move;

                //transform.forward = move;
            }

            if (Input.GetButtonDown("Jump") && m_IsGrounded)
            {
                // Hauteur en fonction de la taille
                float ratioScaleHeight= transform.localScale.x < 0.5f ? 0.5f : 1.0f;

                m_Velocity.y += Mathf.Sqrt(m_JumpHeight * -3.0f * m_GravityValue * ratioScaleHeight);
            }

            m_Velocity.y += m_GravityValue * Time.deltaTime;
            m_Controller.Move(m_Velocity * Time.deltaTime);
            // if (m_GravityValue > 0) transform.localEulerAngles = new Vector3(180.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            if (m_IsGravityChanged)
            {
                //transform.up = m_GravityValue > 0 ? Vector3.down : Vector3.up;
                m_IsGravityChanged = false;
            }
        }

        public void SetGravityInverted(bool inverted)
        {
            m_GravityValue = inverted ? -m_GravityValue : m_GravityValue;
            //gameObject.transform.up = inverted ? Vector3.down : Vector3.up;
            //Vector3 currentLocalAngles = transform.localEulerAngles;
            //transform.localEulerAngles = new Vector3(inverted ? 180f : 0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
            StartCoroutine(GravityInverted());
        }

        IEnumerator GravityInverted()
        {
            yield return new WaitForSeconds(0.5f);
            m_IsGravityChanged = true;
            transform.localEulerAngles = new Vector3(m_GravityValue > 0 ? 180.0f : 0.0f, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

    }

}


