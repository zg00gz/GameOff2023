using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace ScaleTravel
{

    public class VirtualCamController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera Cam1;
        [SerializeField] CinemachineVirtualCamera Cam2;
        [SerializeField] CinemachineVirtualCamera CamEnd;

        [SerializeField] CinemachineBrain Cam;

        public void CheckVirtualCamera(float scaleX)
        {
            // TODO script update sur Cinemachine ?
            if (scaleX < 0.5)
            {
                //Cam1.GetComponent<CinemachineVirtualCamera>().enabled = false;
                //Cam2.GetComponent<CinemachineVirtualCamera>().enabled = true;
                Cam1.Priority = 9;
                Cam2.Priority = 10;
            }
            else
            {
                //Cam1.GetComponent<CinemachineVirtualCamera>().enabled = true;
                //Cam2.GetComponent<CinemachineVirtualCamera>().enabled = false;
                Cam1.Priority = 10;
                Cam2.Priority = 9;
            }
        }

        public void SwitchToEnd()
        {
            Cam.ActiveVirtualCamera.Priority = 9;
            CamEnd.Priority = 10;
        }

    }

}

