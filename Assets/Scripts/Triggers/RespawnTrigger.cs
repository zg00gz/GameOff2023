using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScaleTravel
{

    public class RespawnTrigger : MonoBehaviour
    {

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerController.Instance.Respawn();
            }
        }

    }

}


