using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalGravityArrival : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                Vector3 rotation = other.transform.localEulerAngles;
                rotation.z = other.transform.localEulerAngles.z == 180 ? 0 : 180;

                other.transform.localEulerAngles = rotation;
                // TODO animation

                GetComponent<Collider>().enabled = false;
                PlayerInput.Instance.playerControllerInputBlocked = false;
            }
        }

    }

}
