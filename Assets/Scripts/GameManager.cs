using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


namespace ScaleTravel
{

    public class GameManager : MonoBehaviour
    {
        protected static GameManager s_Instance;
        public static GameManager Instance { get { return s_Instance; } }

        [SerializeField] LevelData m_LevelValues;
        [SerializeField] AudioMixer m_MasterMixer;

        [SerializeField] TMPro.TextMeshProUGUI m_Text_GroupLevelTitle;
        [SerializeField] TMPro.TextMeshProUGUI m_Text_LevelTitle;
        //[SerializeField] TMPro.TextMeshProUGUI m_Text_GoWord;

        [SerializeField] PlayerController m_PlayerController;
        [SerializeField] PlayerInput m_Input;
        [SerializeField] UI_Level m_UI_Level;

        private float m_TimerStartTime;
        private float m_TimerEndTime;
        public bool IsLevelDone;

        public LevelData LevelValues => m_LevelValues;

        void Start()
        {
            if (s_Instance != null)
            {
                Debug.Log("GameManager - gameobject destroyed");
                Destroy(gameObject);
                return;
            }
            s_Instance = this;

            m_PlayerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            m_Input = m_PlayerController.GetComponent<PlayerInput>();

            Cursor.visible = false; // Hide mouse cursor

            LevelValues.SetLanguage(PlayerLocal.Instance.HeroData.Profile.PlayerLanguage);
            m_Text_GroupLevelTitle.text = LevelValues.GroupLevelName;
            m_Text_LevelTitle.text = LevelValues.LevelName;
            //m_Text_GoWord.text = LevelValues.GoWord;

            m_MasterMixer.SetFloat("musicVol", PlayerLocal.Instance.HeroData.Profile.MusicVolume);
            m_MasterMixer.SetFloat("soundVol", PlayerLocal.Instance.HeroData.Profile.SoundVolume);

            StartCoroutine(PlayerControlDelay());
        }
        IEnumerator PlayerControlDelay()
        {
            yield return new WaitForSeconds(0.0f); // TMP
            PlayerInput.Instance.playerControllerInputBlocked = false;
        }

        void Update()
        {
            if (m_Input.Menu)
            {
                m_Input.MenuInput(false);
                Menu();
            }
            if (m_Input.Restart)
            {
                m_Input.RestartInput(false);
                Retry();
            }
        }

        public void Menu()
        {
            //SaveTotalPlayTime();
            SceneManager.LoadScene("Home");
        }
        public void Retry()
        {
            //SaveTotalPlayTime();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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