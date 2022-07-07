using Pathfinding;
using UnityEngine;

public abstract class NPathMovementBase : NEnemyMovement
{
    public float NodeRachedDistance = 0.9f;
    public float UpdatePathInvterval = 0.5f;

    protected Seeker _seeker;
    protected Path TargetPath;

    protected  Vector3 Destination;
    public Vector3 CurrentNode => TargetPath != null ? TargetPath.vectorPath[currPathNodeIndex] : transform.position;
    
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
        if(TargetPath == null ) return;
        while (IsPathNodeReached() && currPathNodeIndex < TargetPath.vectorPath.Count - 1)
            currPathNodeIndex++;
    }
    
    protected bool IsPathNodeReached()
    {
        return Vector2.Distance(transform.position, CurrentNode) < NodeRachedDistance;
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