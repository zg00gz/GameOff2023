using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PlayerInput : MonoBehaviour
    {
        private static PlayerInput s_Instance;
        public static PlayerInput Instance { get { return s_Instance; } }


        [HideInInspector]
        public bool playerControllerInputBlocked;

        private Vector3 m_Move;
        private bool m_Jump;
        private bool m_Pause;
        private bool m_Restart;

        public Vector3 Move
        {
            get
            {
                if (playerControllerInputBlocked)
                    return Vector3.zero;
                return m_Move;
            }
        }

        public bool Jump
        {
            get { return m_Jump && !playerControllerInputBlocked; }
        }

        public bool Pause
        {
            get { return m_Pause; }
        }
        public bool Restart
        {
            get { return m_Restart; }
        }

        void Awake()
        {
            if (s_Instance != null)
            {
                Debug.Log("PlayerInput - gameobject destroyed");
                Destroy(gameObject);
                return;
            }
            s_Instance = this;
        }

        void Update()
        {
            m_Move.Set(Input.GetAxis("Horizontal"), 0, 0);
            m_Jump = Input.GetButton("Jump");
            m_Pause = Input.GetKeyDown(KeyCode.P); // Input.GetButtonDown("Pause");
            m_Restart = Input.GetKeyDown(KeyCode.R); // Input.GetButtonDown("Restart");
        }

    }

}

