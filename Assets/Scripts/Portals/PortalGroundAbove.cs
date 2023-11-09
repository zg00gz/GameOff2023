using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalGroundAbove : MonoBehaviour
    {
        

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Grounded Above"); 
            if (collision.gameObject.CompareTag("Player"))
            {
                //PlayerController.Instance.IsGroundedAbove = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            Debug.Log("Not Grounded Above");
            if (collision.gameObject.CompareTag("Player"))
            {
                //PlayerController.Instance.IsGroundedAbove = false;
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Debug.Log("Grounded Above hit");
        }
    }
}


