using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class RespawnPoint : MonoBehaviour
    {
        [System.Serializable]
        public class ObjectToPosition
        {
            public GameObject Object;
            public Vector3 NewPosition;
        }

        [SerializeField] ObjectToPosition[] m_ListToMove; // TODO script indépendant => Trigger move / destroy / add ?
        // TODO option change camera => script indépendant ?

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                PlayerController.Instance.RespawnPoint = transform.position;
            }
        }
    }

}

