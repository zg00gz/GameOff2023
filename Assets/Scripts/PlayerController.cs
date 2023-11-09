using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaleTravel
{

    public class PlayerController : MonoBehaviour
    {
        private static PlayerController s_Instance;
        public static PlayerController Instance { get { return s_Instance; } }


        [SerializeField] float m_Speed = 5.0f;
        [SerializeField] float m_Acceleration = 15.0f;
        [SerializeField] float m_JumpHeight = 4.0f;
        [SerializeField] float m_GravityValue = -9.81f;

        [SerializeField] bool m_IsGrounded;
        [SerializeField] bool m_IsCollided;
        [SerializeField] bool m_IsJumping;
        [SerializeField] Vector3 m_Velocity;
        [SerializeField] Vector3 m_PrevVelocity;

        Rigidbody m_Rigidbody;
        PlayerInput m_Input; 


        void Awake()
        {
            if (s_Instance != null)
            {
                Debug.Log("PlayerController - gameobject destroyed");
                Destroy(gameObject);
                return;
            }
            s_Instance = this;

            m_Input = GetComponent<PlayerInput>();
            m_Rigidbody = GetComponent<Rigidbody>();
            m_IsGrounded = true;
        }

        void FixedUpdate()
        {
            UpdateVelocity();
            UpdateDirection();
            UpdateJump();

            m_PrevVelocity = m_Rigidbody.velocity;
        }

        private void UpdateVelocity()
        {
            m_Velocity = m_Rigidbody.velocity;
            Vector3 move = m_Input.Move;

            if (move.magnitude >= 0.1f)
            {
                // Vitesse en fonction de la taille
                float ratioScaleSpeed = transform.localScale.x < 1 ? 1 / transform.localScale.x : 1.0f;
                float maxSpeed = m_Speed + ratioScaleSpeed - 1;

                m_Velocity += move * m_Acceleration * Time.fixedDeltaTime;
                m_Velocity.x = Mathf.Clamp(m_Velocity.x, -maxSpeed, maxSpeed);
                m_Rigidbody.velocity = m_Velocity;
            }
            else
            {
                m_Velocity.x = 0f;
                m_Rigidbody.velocity = m_Velocity;
            }
        }

        private void UpdateJump()
        {
            if (m_IsGrounded && m_Input.Jump)
            {
                m_IsJumping = true;

                // Hauteur en fonction de la taille
                float ratioScaleHeight = transform.localScale.x < 0.5f ? 0.5f : 1.0f;

                m_Rigidbody.AddForce(transform.up * (m_JumpHeight + ratioScaleHeight - 1), ForceMode.VelocityChange);
            }

            if (m_IsCollided && m_IsJumping) // Mathf.Round(m_Velocity.y) == 0
            {
                m_Velocity.x = 0f;
                m_Rigidbody.velocity = m_Velocity;
            }
        }

        private void UpdateDirection()
        {
            Vector3 move = m_Input.Move;

            if (m_GravityValue > 0)
            {
                Vector3 direction = transform.localEulerAngles;

                if (move != Vector3.zero)
                {
                    if (move.x < 0)
                        direction.y = 270;
                    else
                        direction.y = 90;
                }
                transform.localEulerAngles = direction;
            }
            else if (move != Vector3.zero)
            {
                transform.forward = move;
            }
        }


        public void SetGravityInverted()
        {
            m_GravityValue = -m_GravityValue;
            Physics.gravity = new Vector3(0, m_GravityValue, 0);

            Vector3 rotation = transform.localEulerAngles;
            rotation.z = m_GravityValue > 0 ? 180f : 0f;
            StartCoroutine(SetLocalEulerAngles(rotation));
        }
        IEnumerator SetLocalEulerAngles(Vector3 rotation)
        {
            yield return new WaitForSeconds(0.5f);
            transform.localEulerAngles = rotation;
            // TODO animation 180° pour remplacer coroutine
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                m_IsCollided = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                m_IsCollided = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                m_IsGrounded = true;
                m_IsJumping = false;
                m_Velocity.y = 0f;
                m_Rigidbody.velocity = m_Velocity;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                m_IsGrounded = false;
            }
        }

    }

}


