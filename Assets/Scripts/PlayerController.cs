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
        public Vector3 RespawnPoint;
        [SerializeField] FadeTransition m_TransitionScript;

        [SerializeField] bool m_IsGrounded;
        [SerializeField] bool m_IsCollided;
        public bool IsJumping;
        [SerializeField] Vector3 m_Velocity;
        //[SerializeField] Vector3 m_PrevVelocity;

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
            m_Input.playerControllerInputBlocked = true;
        }

        void Update()
        {
            if (transform.position.y < -5 || transform.position.y > 15)
            {
                Respawn();
            }
        }

        void FixedUpdate()
        {
            if(!m_Input.playerControllerInputBlocked)
            {
                UpdateVelocity();
                UpdateDirection();
                UpdateJump();
            }

            //m_PrevVelocity = m_Rigidbody.velocity;
        }

        private void UpdateVelocity()
        {
            m_Velocity = m_Rigidbody.velocity;
            Vector3 move = m_Input.Move;

            if (move.magnitude >= 0.1f)
            {
                // Vitesse en fonction de la taille
                //float ratioScaleSpeed = transform.localScale.x < 1 ? 1 / transform.localScale.x : 1.0f;
                float ratioScaleSpeed = transform.localScale.x < 1 ? 1 / transform.localScale.x : transform.localScale.x;
                ratioScaleSpeed = Mathf.Clamp(ratioScaleSpeed, 1.0f, 1.5f);

                float maxSpeed = m_Speed + ratioScaleSpeed - 1;
                //if (IsJumping) maxSpeed /= 2;

                m_Velocity += move * m_Acceleration * Time.fixedDeltaTime;
                //m_Rigidbody.AddForce(move * m_Acceleration, ForceMode.Acceleration);
                m_Velocity.x = Mathf.Clamp(m_Velocity.x, -maxSpeed, maxSpeed);
                m_Rigidbody.velocity = m_Velocity;
            }
            else
            {
                m_Velocity.x = 0f;
                m_Rigidbody.velocity = m_Velocity;
            }

            if (m_IsCollided && IsJumping) // !m_IsGrounded && 
            {
                //m_Rigidbody.AddForce(-move * 1.5f, ForceMode.VelocityChange);
                m_Velocity.x = 0f;
                m_Rigidbody.velocity = m_Velocity;
            }
        }

        private void UpdateJump()
        {
            if (!IsJumping && m_IsGrounded && m_Input.Jump)
            {
                IsJumping = true;

                // Hauteur en fonction de la taille
                //float ratioScaleHeight = transform.localScale.x < 1.0f ? transform.localScale.x : 1.0f;
                float ratioScaleHeight = Mathf.Clamp(transform.localScale.x, 0.5f, 1.5f);

                m_Rigidbody.AddForce(transform.up * (m_JumpHeight + ratioScaleHeight), ForceMode.VelocityChange);
            }
            else
            {
                m_Input.JumpInput(false);
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

        private void Respawn()
        {
            m_TransitionScript.StartFadeOut();
            if (m_GravityValue > 0) SetGravityInverted();
            SetPosition(RespawnPoint);
            transform.forward = Vector3.right;
        }

        public void SetGravityInverted()
        {
            m_GravityValue = -m_GravityValue;
            m_Rigidbody.velocity = Vector3.zero;
            Physics.gravity = new Vector3(0, m_GravityValue, 0);
        }

        public void SetPosition(Vector3 position)
        {
            SetKinematic(true);
            transform.position = position;
            SetKinematic(false);
        }

        public void SetKinematic(bool value)
        {
            m_Input.playerControllerInputBlocked = value;
            m_Rigidbody.isKinematic = value;
            m_Input.JumpInput(false);
            m_Input.ActionInput(false);
        }


        private void OnCollisionStay(Collision collision)
        {
            m_IsCollided = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            m_IsCollided = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                m_IsGrounded = true;
                IsJumping = false;
                if(!m_Input.playerControllerInputBlocked)
                {
                    m_Velocity.y = 0f;
                    m_Rigidbody.velocity = m_Velocity;
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                m_IsGrounded = true;
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


