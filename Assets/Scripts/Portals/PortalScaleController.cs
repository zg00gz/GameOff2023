using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PortalScaleController : MonoBehaviour
{
    [SerializeField] bool m_IsActive;
    [SerializeField] float m_Delay;
    [SerializeField] float m_Duration;
    [SerializeField] float m_Speed = 2f;
    [SerializeField] Vector3 m_TargetScale;

    public CinemachineBrain Cam;
    [SerializeField] CinemachineVirtualCamera Cam1;
    [SerializeField] CinemachineVirtualCamera Cam2;

    GameObject m_Target;
    bool m_IsScaleUp;
    bool m_IsActionRunning;


    void Start()
    {
        // TODO ajouter particule system avec les - et/ou sens des particules ?
    }

    void Update()
    {
        if(m_IsActive)
        {
            if(!m_IsActionRunning)
            {
                if (m_Delay > 0)
                {
                    // TODO si delay => coroutine + isactive false => à la fin de la coroutine m_Target = null
                }
                if (m_Duration > 0)
                {

                }

                m_IsScaleUp = m_Target.transform.localScale.x < m_TargetScale.x;
                m_IsActionRunning = true;
            }
            else
            {
                if (!m_IsScaleUp)
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
        Debug.Log("Scaled !");
        m_Target.transform.localScale = m_TargetScale;
        m_IsActive = false;
        m_IsActionRunning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player enter");
            m_Target = other.gameObject;
            m_IsActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player exit");
            m_IsActive = false;
            if(m_Delay == 0f && m_Duration == 0f) m_Target = null;


            // TODO script update sur Cinemachine ?
            if (m_TargetScale.x < 0.5)
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
    }
}
