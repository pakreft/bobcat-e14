using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using VehiclePhysics.Specialized;

public class Automation : MonoBehaviour
{
    [SerializeField] private GameObject m_destination;
    [SerializeField] private NavMeshAgent m_agent;
    [SerializeField] private VPHydraulicTrackedVehicleController m_vehicle;

    private TextMeshProUGUI m_label = null;
    private bool m_followingActive = false;
    
    private void Start()
    {
        m_label = GetComponentInChildren<TextMeshProUGUI>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void FixedUpdate()
    {
        if (m_followingActive)
        {
            
        }
    }

    private void TaskOnClick()
    {
        if (m_label.color == Color.white)
        {
            m_agent.transform.position = m_vehicle.transform.position;
            m_agent.destination = m_destination.transform.position;
            m_label.color = new Color(255, 190, 0);
            m_followingActive = true;
        }
        else if (m_followingActive)
        {
            m_agent.destination = m_agent.transform.position;
            m_label.color = Color.white;
            m_followingActive = false;
        }
    }

    public void SetAgentDestination(Vector3 pos)
    {
        m_destination.SetActive(true);
        m_destination.transform.position = new Vector3(pos.x, m_destination.transform.position.y, pos.z);

        if (m_followingActive)
        {
            m_agent.destination = m_destination.transform.position;
        }
        else
        {
            m_label.color = Color.white;
        }
    }
}
