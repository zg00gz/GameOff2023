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

        // Avant d'afficher l'aide, on vérifie le contrôleur utilisé 
        public void LastDeviceUsed()
        {
            // PlayerInput.Instance.
        }
    }

}