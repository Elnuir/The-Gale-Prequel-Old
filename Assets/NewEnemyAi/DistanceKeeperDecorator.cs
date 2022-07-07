using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DistanceKeeperDecorator : MovementBaseX
{
    public MovementBaseX Movement;
    public AchievementHandler.MobType[] NeighborTypes;
    public float MinDistance = 3f;
    public float ScanInterval = 0.25f;

    private Collider2D[] buffer = new Collider2D[100];
    private Transform[] neighbors = new Transform[0];

    public override bool IsMoving => Movement.IsMoving;
    private void Update()
    {
        if (ActionEx.CheckCooldown(Update, ScanInterval))
            neighbors = GetNeighbors().ToArray();
    }


    private IEnumerable<Transform> GetNeighbors()
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, MinDistance, buffer);

        return buffer
        .Take(count)
        .Select(a => a.transform)
        .Where(a =>
        {
            if (a == transform) return false;
            if (a.gameObject.TryGetComponent<AchievementHandler>(out var handler))
                return NeighborTypes.Contains(handler.EnemyType);
            return false;
        });
    }

    public override void MoveTo(Vector3 target)
    {
        if (neighbors.Length == 0)
            Movement.MoveTo(target);
        else
            Movement.MoveTo(GetPointToAvoid(neighbors));
    }

    private Vector2 GetPointToAvoid(Transform[] neighbors)
    {
        var NearestLeftNeighbor = neighbors.Where(IsOnLeft).OrderBy(t => Vector2.Distance(transform.position, t.position)).FirstOrDefault();
        var NearestRightNeighbor = neighbors.Where(IsOnRight).OrderBy(t => Vector2.Distance(transform.position, t.position)).FirstOrDefault();

        if (NearestLeftNeighbor != null)
        {
            if (NearestRightNeighbor != null)
            {
                float x = (NearestLeftNeighbor.position.x + NearestRightNeighbor.position.x) / 2;
                return new Vector2(x, transform.position.y);
            }
            return new Vector2(transform.position.x + MinDistance, transform.position.y);
        }
        if (NearestRightNeighbor != null)
            return new Vector2(transform.position.x - MinDistance, transform.position.y);

        return transform.position;
    }

    private bool IsOnLeft(Transform target)
    {
        return target.position.x < transform.position.x;
    }

    private bool IsOnRight(Transform target)
    {
        return target.position.x > transform.position.x;
    }

    public override void StopMovement()
    {
        Movement.StopMovement();
    }
}
