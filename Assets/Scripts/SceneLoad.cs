using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace ScaleTravel
{
    public class SceneLoad : MonoBehaviour
    {
        public UnityEvent OnLoad = new UnityEvent();

        void Awake()
        {
            SceneManager.sceneLoaded += PlayEvent;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= PlayEvent;
        }

        private void PlayEvent(Scene scene, LoadSceneMode mode)
        {
            OnLoad.Invoke();
        }
    }
}

