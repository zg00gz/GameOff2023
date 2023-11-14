using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{
    public class PortalDoor : MonoBehaviour
    {
        [SerializeField] Material m_LockMaterial;
        [SerializeField] Material m_UnlockMaterial;

        [SerializeField] GameObject m_WallForce;
        [SerializeField] MeshRenderer m_ForceBottom;
        [SerializeField] MeshRenderer m_ForceTop;


        public void SetUnlocked()
        {
            m_WallForce.SetActive(false);
            m_ForceBottom.material = m_UnlockMaterial;
            m_ForceTop.material = m_UnlockMaterial;
        }

        public void SetLocked()
        {
            m_WallForce.SetActive(true);
            m_ForceBottom.material = m_LockMaterial;
            m_ForceTop.material = m_LockMaterial;
        }
    }
}