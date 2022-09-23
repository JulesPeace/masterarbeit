using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastLogic : MonoBehaviour
{
    public float distance;
    private RaycastHit hit;
    private Ray ray;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        QuestDebugLogic.instance.log("Nichts getroffen!");
        ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        //Debug.DrawRay(ray.origin, ray.direction * distance,Color.green,Time.deltaTime,true);
        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.tag == "ProjectionArea")
            {
                lineRenderer.endColor = Color.green;
            }
            else
            {
                lineRenderer.endColor = Color.red;
            }
            lineRenderer.SetPosition(0,ray.origin);
            lineRenderer.SetPosition(1, hit.point);
            //lineRenderer.startColor = Color.red;
            //lineRenderer.endColor=Color.green;
            lineRenderer.enabled = true;
            QuestDebugLogic.instance.log("Treffer mit "+hit.collider.tag+"!");
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
