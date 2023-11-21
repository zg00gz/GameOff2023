using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


namespace ScaleTravel
{

    public class VirtualCamController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera CamNormal;
        [SerializeField] CinemachineVirtualCamera CamSmall;
        [SerializeField] CinemachineVirtualCamera CamEnd;

        [SerializeField] CinemachineBrain Cam;

        public void CheckVirtualCamera(float scaleX)
        {
            if (scaleX < 0.5)
            {
                CamNormal.Priority = 9;
                CamSmall.Priority = 10;
            }
            else
            {
                CamNormal.Priority = 10;
                CamSmall.Priority = 9;
            }
        }

        public void SwitchToEnd()
        {
            //Cam.ActiveVirtualCamera.Priority = 9;
            CamEnd.Priority = 11;
        }

        public void SwitchCollider(Collider collider)
        {
            CamNormal.GetComponent<CinemachineConfiner>().m_BoundingVolume = collider;
            CamSmall.GetComponent<CinemachineConfiner>().m_BoundingVolume = collider;
        }

    }

}

