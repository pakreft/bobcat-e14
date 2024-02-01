using UnityEngine;
using UnityEngine.UI;

public class DroneView : MonoBehaviour
{
    [SerializeField] private Camera m_droneCamera;
    [SerializeField] private Automation m_automation;

    private RectTransform m_rectTransform;
    private RawImage m_rawImage;
    private Vector2 m_droneViewMousePos = new Vector2();
    
    private void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();
        m_rawImage = GetComponent<RawImage>();
    }
    
    private void Update()
    {
        // 0 is the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // Step 2.1: Map cursor position to local rectangle coordinates
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, Input.mousePosition, null,
                out m_droneViewMousePos);
            
            // Step 2.2: Map local rectangle coordinates to view port coordinates
            m_droneViewMousePos.x = (m_droneViewMousePos.x / m_rectTransform.rect.width) + m_rectTransform.pivot.x;
            m_droneViewMousePos.y = (m_droneViewMousePos.y / m_rectTransform.rect.height) + m_rectTransform.pivot.y;
            
            // Step 2.3
            // Add the uv displacement
            m_droneViewMousePos.x += m_rawImage.uvRect.x;
            // Revert the stretch
            m_droneViewMousePos.x *= m_rawImage.uvRect.width;

            // Check if mouse was clicked in drone view image, if yes cast ray
            var con1 = m_droneViewMousePos.x > 0.0f;
            var con2 = m_droneViewMousePos.x < 1.0f;
            var con3 = m_droneViewMousePos.y > 0.0f;
            var con4 = m_droneViewMousePos.x < 1.0f;
            
            if (con1 && con2 && con3 && con4)
            {
                // Step 3.1: Create ray from drone camera through mouse position on rect
                Ray ray = m_droneCamera.ViewportPointToRay(m_droneViewMousePos);
                
                // Step 3.2: Create plane on same level as construction site
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                // Step 3.3: Intersects a ray with the plane.
                // This function sets enter to the distance along the ray, where it intersects the plane.
                plane.Raycast(ray, out float enterDistance);

                // Step 3.4: Return the point at enterDistance units along the ray
                Vector3 hitPoint = ray.GetPoint(enterDistance);

                // Step 4: Move agent
                m_automation.SetAgentDestination(hitPoint);
            }
        }
    }
}
