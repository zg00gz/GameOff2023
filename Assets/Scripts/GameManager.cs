using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ScaleTravel
{

    public class GameManager : MonoBehaviour
    {
        protected static GameManager s_Instance;
        public static GameManager Instance { get { return s_Instance; } }

        [SerializeField] LevelData m_LevelValues;
        [SerializeField] TMPro.TextMeshPro m_Text_GroupLevelTitle;
        [SerializeField] TMPro.TextMeshPro m_Text_LevelTitle;

        [SerializeField] UI_Level m_UI_Level;

        private float m_TimerStartTime;
        private float m_TimerEndTime;
        public bool IsLevelDone;

        public LevelData LevelValues => m_LevelValues;

        void Update()
        {
            if (PlayerInput.Instance.Restart)
            {
                Retry();
            }
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Avant d'afficher l'aide, on vérifie le contrôleur utilisé 
        public void LastDeviceUsed()
        {
            // PlayerInput.Instance.
        }
    }

}