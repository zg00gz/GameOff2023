using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace ScaleTravel
{

    public class GameManager : MonoBehaviour
    {
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
    }

}