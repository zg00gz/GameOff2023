using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScaleTravel
{

    public class PortalScale : MonoBehaviour
    {
        [SerializeField] bool m_IsActive;
        [SerializeField] float m_Delay;
        [SerializeField] float m_Duration;
        [SerializeField] float m_Speed = 2f;
        [SerializeField] Vector3 m_TargetScale;
        [SerializeField] GameObject m_TypeScaleUp;
        [SerializeField] GameObject m_TypeScaleDown;

        GameObject m_Target;
        bool m_IsScalingUp;
        bool m_IsActionRunning;

        VirtualCamController m_VirtualCamScript;
        private AudioSource m_AudioSource;

        void Start()
        {
            m_AudioSource = GetComponent<AudioSource>();
            m_VirtualCamScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<VirtualCamController>();
            DisplayType(PlayerController.Instance.transform.localScale.x);
        }

        void Update()
        {
            if (m_IsActive)
            {
                if (!m_IsActionRunning)
                {
                    if (m_Delay > 0)
                    {
                        // TODO si delay => coroutine + isactive false => � la fin de la coroutine m_Target = null
                    }
                    if (m_Duration > 0)
                    {

                    }

                    m_IsScalingUp = m_Target.transform.localScale.x < m_TargetScale.x;
                    m_AudioSource.Play();
                    m_IsActionRunning = true;
                }
                else
                {
                    if (!m_IsScalingUp)
                    {
                        if (m_Target.transform.localScale.x <= m_TargetScale.x)
                        {
                            Scaled();
                            return;
                        }
                        m_Target.transform.localScale -= m_TargetScale * Time.deltaTime * m_Speed;
                    }
                    else
                    {
                        if (m_Target.transform.localScale.x >= m_TargetScale.x)
                        {
                            Scaled();
                            return;
                        }
                        m_Target.transform.localScale += m_TargetScale * Time.deltaTime * m_Speed;
                    }
                }
            }
        }

        private void Scaled()
        {
            //Debug.Log("Scaled !");
            m_Target.transform.localScale = m_TargetScale;
            m_VirtualCamScript.CheckVirtualCamera(m_Target.transform.localScale.x);
            m_IsActive = false;
            m_IsActionRunning = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_Target = other.gameObject;
                m_IsActive = true;
                HideType();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && other.isTrigger)
            {
                m_IsActive = false;
                m_VirtualCamScript.CheckVirtualCamera(other.transform.localScale.x);

                if (m_Delay == 0f && m_Duration == 0f)
                {
                    UpdateAllDisplayType(m_Target.transform.localScale.x);
                    m_Target = null;
                }
            }
        }

        public void DisplayType(float targetScaleX)
        {
            if (m_TargetScale.x > targetScaleX)
            {
                m_TypeScaleUp.SetActive(true);
                m_TypeScaleDown.SetActive(false);
            }
            else if (m_TargetScale.x < targetScaleX)
            {
                m_TypeScaleDown.SetActive(true);
                m_TypeScaleUp.SetActive(false);
            }
            else
            {
                // Same scale than the player
                HideType();
            }
        }

        private void UpdateAllDisplayType(float targetScaleX)
        {
            var portals = GameObject.FindGameObjectsWithTag("PortalScale");

            foreach (var portal in portals)
            {
                portal.GetComponent<PortalScale>().DisplayType(targetScaleX);
            }
        }

        private void HideType()
        {
            m_TypeScaleDown.SetActive(false);
            m_TypeScaleUp.SetActive(false);
        }


    }

}
