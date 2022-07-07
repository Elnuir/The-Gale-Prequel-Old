using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(Seeker))]
public abstract class PathMovementBaseX : MovementBaseX
{
    public float NodeRachedDistance = 0.9f;
    public float UpdatePathInvterval = 0.5f;

    protected Seeker _seeker;
    public Path TargetPath { get; protected set; }
    public override bool IsMoving => Destination != null && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 0.3f;

    public Vector3 Destination { get; protected set; }
    public Vector3? CurrentNode => TargetPath?.vectorPath[currPathNodeIndex];

    private int currPathNodeIndex;

    protected virtual void Start()
    {
        _seeker = GetComponent<Seeker>();
        InvokeRepeating("UpdatePath", 0f, UpdatePathInvterval);
    }

    protected virtual void Update()
    {
        GoNextNode();
    }

    void GoNextNode()
    {
        if (TargetPath == null) return;
        while (IsPathNodeReached() && currPathNodeIndex < TargetPath.vectorPath.Count - 1)
            currPathNodeIndex++;
    }

    protected bool IsPathNodeReached()
    {
        if (CurrentNode is Vector3 node)
            return GetDistanceTo(node) < NodeRachedDistance;
        return false;
    }

    private float GetDistanceTo(Vector2 target)
    {
        return Vector2.Distance(transform.position, target);
    }

    void UpdatePath()
    {
        if (_seeker.IsDone())
            _seeker.StartPath(transform.position, Destination, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        TargetPath = p;
        currPathNodeIndex = 0;
    }
}
