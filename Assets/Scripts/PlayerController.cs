using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaleTravel
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float m_Speed = 4f;

        Rigidbody m_Rigidbody;
        Vector3 m_Movement;
        
        void Start()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        }
        
        void FixedUpdate()
        {
            float horizontal = Input.GetAxis("Horizontal");
            m_Movement.Set(horizontal, 0f, 0f);
            m_Movement.Normalize();

            //bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);

            if(m_Movement.magnitude >= 0.1f)
            {
                m_Rigidbody.MovePosition(transform.position + m_Movement * Time.deltaTime * m_Speed);
            }
        }
    }

}


