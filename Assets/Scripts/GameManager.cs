using System.Collections;
using System.Linq;
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
        [SerializeField] TMPro.TextMeshProUGUI m_Text_GoWord;
        [SerializeField] TMPro.TextMeshProUGUI m_Text_ScaleLevel;
        //[SerializeField] float m_LevelStartDelay = 3.0f; // TMP


        [SerializeField] PlayerController m_PlayerController;
        [SerializeField] PlayerInput m_Input;
        [SerializeField] UI_Level m_UI_Level;

        private float m_LoadLevelTime;
        private float m_TimerStartTime;
        private float m_TimerEndTime;
        public bool IsLevelDone;

        public LevelData LevelValues => m_LevelValues;
        private PlayerLocal.Level m_LastPlayerLevelValues;

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
            m_Text_GoWord.text = LevelValues.GoWord;
            m_Text_ScaleLevel.text = LevelValues.LevelScale;
            m_LastPlayerLevelValues = PlayerLocal.Instance.HeroData.Levels.Where(d => d.LevelID == LevelValues.LevelID).FirstOrDefault();

            m_LoadLevelTime = Time.time;
            m_MasterMixer.SetFloat("musicVol", PlayerLocal.Instance.HeroData.Profile.MusicVolume);
            m_MasterMixer.SetFloat("soundVol", PlayerLocal.Instance.HeroData.Profile.SoundVolume);

            //StartCoroutine(LevelStartDelay()); // TMP
        }
        
        //IEnumerator LevelStartDelay()
        //{
        //    yield return new WaitForSeconds(m_LevelStartDelay);
        //    LevelStart();
        //}

        void Update()
        {
            if (m_Input.Menu)
            {
                m_Input.MenuInput(false);
                Home();
            }
            if (m_Input.Restart)
            {
                m_Input.RestartInput(false);
                Retry();
            }
        }

        public void Home()
        {
            SaveTotalPlayTime();
            SceneManager.LoadScene("Home");
        }
        public void Retry()
        {
            SaveTotalPlayTime();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        public void NextLevel()
        {
            SaveTotalPlayTime();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

        // Démarre après l'animation d'introduction
        public void LevelStart()
        {
            m_Input.playerControllerInputBlocked = false;

            m_UI_Level.StartLevelScreen();
            if (m_LastPlayerLevelValues != null) DisplayTimer();
        }

        private void DisplayTimer()
        {
            m_TimerStartTime = Time.time;
            
            string textTime = PlayerLocal.Instance.FormatTime(LevelValues.RunCupTime[2]);
            m_UI_Level.TimerCup(1);
            m_UI_Level.DisplayTimer(textTime);

            StartCoroutine(UpdateTimer(LevelValues.RunCupTime[2]));
        }
        IEnumerator UpdateTimer(float timeRemaining)
        {
            var currentCup = 1;

            while (timeRemaining > 0 && !IsLevelDone)
            {
                yield return new WaitForSeconds(1.0f);
                timeRemaining--;
                string time = PlayerLocal.Instance.FormatTime(timeRemaining);
                m_UI_Level.UpdateTimer(time);

                var cupTime = LevelValues.RunCupTime[2] - timeRemaining;

                if (currentCup < 2 && cupTime > LevelValues.RunCupTime[0])
                {
                    currentCup = 2;
                    m_UI_Level.TimerCup(2);
                    yield return null;
                }
                if (currentCup < 3 && cupTime > LevelValues.RunCupTime[1])
                {
                    currentCup = 3;
                    m_UI_Level.TimerCup(3);
                }
            }
            m_UI_Level.HideTimer();
        }

        public void LevelDone()
        {
            IsLevelDone = true;
            
            //if (m_MusicRunEnd)
            //{
            //    m_AudioSource.Stop();
            //    m_MusicRunEnd.Play();
            //}

            // SavePlayerLevel
            m_TimerEndTime = Time.time;
            float time = m_TimerEndTime - m_TimerStartTime;
            string displayTime = PlayerLocal.Instance.FormatTime(time, true);

            PlayerLocal.Instance.SaveLevel(
                PlayerLocal.Instance.HeroData.Profile.PlayerID,
                LevelValues.LevelID,
                time,
                displayTime
            );
            
            m_UI_Level.EndLevelScreen(time, displayTime);
        }

        public void SaveTotalPlayTime()
        {
            PlayerLocal.Instance.HeroData.Profile.TotalPlayedTime += Time.time - m_LoadLevelTime;
            PlayerLocal.Instance.SaveProfile(PlayerLocal.Instance.HeroData.Profile);
        }
    }

}