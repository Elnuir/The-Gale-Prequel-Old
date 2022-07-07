using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PatrolTargetProvider : TargetProviderBase
{
    public WaypointContainer WpContainer;
    public float IsReachedDistance = 0.5f;

    private List<Transform> Waypoints => WpContainer.waypoints;
    public Transform CurrentWp => Waypoints[currentWpIndex];
    private int currentWpIndex;

    private void Start()
    {
        PickNextWaypoint();
    }

    bool IsWaypointReached(Transform wp)
    {
        return Vector2.Distance(transform.position, wp.transform.position) < IsReachedDistance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Teleport")
        {
            currentWpIndex = Waypoints.Select((w, i) => i).Where(i => i != currentWpIndex).OrderBy(_ => Random.Range(0, 100)).First();
        }
    }

    private void PickNextWaypoint()
    {
        currentWpIndex = Random.Range(0, Waypoints.Count);
    }
    public override Transform GetTarget()
    {
        if (IsWaypointReached(CurrentWp))
            PickNextWaypoint();

        return CurrentWp;
    }
}