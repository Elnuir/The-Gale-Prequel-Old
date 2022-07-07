using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
    [HideInInspector] public List<Transform> waypoints =new List<Transform>();
    
    // Start is called before the first frame update
    void Start()
    {
        waypoints.AddRange(transform.GetComponentsInChildren<Transform>().Where(t => t.transform != transform));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
