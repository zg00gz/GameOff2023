using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
#endif


namespace ScaleTravel
{
    public class PlayerInput : MonoBehaviour
    {
        private static PlayerInput s_Instance;
        public static PlayerInput Instance { get { return s_Instance; } }


        [HideInInspector]
        public bool playerControllerInputBlocked;

        private Vector3 m_Move;
        private Vector3 m_MoveVertical;
        
        private bool m_Action;
        private bool m_Jump;
        private bool m_Menu;
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

        public Vector3 MoveVertical
        {
            get
            {
                if (playerControllerInputBlocked)
                    return Vector3.zero;
                return m_MoveVertical;
            }
        }

        public bool Action
        {
            get { return m_Action && !playerControllerInputBlocked; }
        }

        public bool Jump
        {
            get { return m_Jump && !playerControllerInputBlocked; }
        }

        public bool Menu
        {
            get { return m_Menu; }
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

#if ENABLE_INPUT_SYSTEM

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnMoveVertical(InputValue value)
        {
            MoveVerticalInput(value.Get<Vector2>());
        }

        public void OnAction(InputValue value)
        {
            ActionInput(value.isPressed);
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnMenu(InputValue value)
        {
            MenuInput(value.isPressed);
        }

        public void OnRestart(InputValue value)
        {
            // A savoir : le select du gamepad est utilisé dans les navigateurs pour quitter le mode jeu
            // =>   prévoir un autre bouton
            //      Autre solution : afficher la souris pour utiliser les boutons comme dans Home
            //      Autre solution : Mettre en pause avec Start puis choisir les actions Menu/Restart/Autre et en faisant apparaître la souris
            RestartInput(value.isPressed);
        }

#endif

        public void MoveInput(Vector2 newMoveDirection)
        {
            m_Move = newMoveDirection;
        }

        public void MoveVerticalInput(Vector2 newMoveDirection)
        {
            m_MoveVertical = newMoveDirection;
        }

        public void ActionInput(bool newAction)
        {
            m_Action = newAction;
        }

        public void JumpInput(bool newJumpState)
        {
            m_Jump = newJumpState;
        }

        public void MenuInput(bool newMenuState)
        {
            m_Menu = newMenuState;
        }

        public void RestartInput(bool newRestartState)
        {
            m_Restart = newRestartState;
        }

    }

}

