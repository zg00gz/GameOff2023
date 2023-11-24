using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;


namespace ScaleTravel
{

    public class HomeManager : MonoBehaviour
    {
        private static HomeManager s_Instance;
        public static HomeManager Instance { get { return s_Instance; } }


        private UI_Home m_UI_Home;
        private PlayerInput m_Input;

        public bool IsHomeLoaded;

        void Start()
        {
            if (s_Instance != null)
            {
                Debug.Log("HomeManager - gameobject destroyed");
                Destroy(gameObject);
                return;
            }
            s_Instance = this;

            m_UI_Home = GetComponent<UI_Home>();
            m_Input = GetComponent<PlayerInput>();
        }

        void Update()
        {
            if(IsHomeLoaded)
            {
                /* Pas nécessaire :
                if (m_Input.Menu)
                {
                    m_Input.MenuInput(false);
                    OpenCloseParams();
                }
                if (m_Input.Restart)
                {
                    m_Input.RestartInput(false);
                    OpenCloseProfiles();
                }
                */
            }

            /* Ne fonctionne pas :
            Vector2 move = m_Input.Move;
            if (move.sqrMagnitude >= 0.1f)
            {
                Vector2 currentPosition = Mouse.current.position.ReadValue();
                Vector2 newPosition = currentPosition + (100.0f * move * Time.deltaTime);
                Mouse.current.WarpCursorPosition(newPosition);
            }

            Vector2 moveVertical = m_Input.MoveVertical;
            if (moveVertical.sqrMagnitude >= 0.1f)
            {
                Vector2 currentPosition = Mouse.current.position.ReadValue();
                Vector2 newPosition = currentPosition + (100.0f * moveVertical * Time.deltaTime);
                Mouse.current.WarpCursorPosition(newPosition);
            }
            */

#if !UNITY_WEBGL
            if (Input.GetKey("escape"))
            {

                Debug.Log("Quit (Escape)");
                Application.Quit();
            }
#endif
        }

        private void OpenCloseParams()
        {
            m_UI_Home.DisplayUpdateProfile();
        }

        private void OpenCloseProfiles()
        {
            m_UI_Home.DisplayProfiles();
        }

        public bool IsGamepadUsed()
        {
            var gamepad = Gamepad.current;

            if (gamepad == null)
                return false;
            if (gamepad.displayName != "Keyboard" && gamepad.displayName != "Mouse")
                return true;

            //Debug.Log(Gamepad.current.displayName);
            return false;
        }

    }

}