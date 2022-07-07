using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAntihit : MonoBehaviour
{
    public float CeilingCheckersInterval;
    public float CeilingCheckersDistance;
    public Vector2 CeilingCheckersOffset;
    public Rect MainCeilingChecker = new Rect(0, 0, 1, 1);
    public LayerMask WhatIsCeiling;

    private Vector2 ActualCenter => (Vector2)transform.position + CeilingCheckersOffset * new Vector2(transform.right.x, 1);
    private Vector2 LeftCheckerPos => ActualCenter - new Vector2(0, CeilingCheckersInterval / 2);
    private Vector2 RightCheckerPos => ActualCenter + new Vector2(0, CeilingCheckersInterval / 2);
    private Rect ActualMainCeilingChecker => new Rect(
        (Vector2)transform.position + new Vector2(MainCeilingChecker.position.x * transform.right.x, MainCeilingChecker.position.y)
        , new Vector2(MainCeilingChecker.size.x * transform.right.x, MainCeilingChecker.size.y));

    private Player _player;

    public bool DrawGizmos;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRightTeleportRequired())
            RightTeleport();

        else if (IsLeftTeleportRequired())
            LeftTeleport();
    }

    private void RightTeleport()
    {
        transform.position = new Vector2(transform.position.x, RightCheckerPos.y);
    }

    private void LeftTeleport()
    {
        transform.position = new Vector2(transform.position.x, LeftCheckerPos.y);
    }

    private bool IsRightTeleportRequired()
    {
        return _player.GetDashStatus() && CheckCeiling(ActualMainCeilingChecker) && CheckCeiling(LeftCheckerPos) && !CheckCeiling(RightCheckerPos);
    }

    private bool IsLeftTeleportRequired()
    {
        return _player.GetDashStatus() &&  CheckCeiling(ActualMainCeilingChecker) && !CheckCeiling(LeftCheckerPos) && CheckCeiling(RightCheckerPos);
    }

    private bool CheckCeiling(Vector2 start)
    {
        return Physics2D.Raycast(start, transform.right, CeilingCheckersDistance, WhatIsCeiling);
    }


    private bool CheckCeiling(Rect area)
    {
        return Physics2D.OverlapArea(area.min, area.max, WhatIsCeiling);
    }

    private void OnDrawGizmos()
    {
        if (!DrawGizmos) return;
        Gizmos.color = CheckCeiling(LeftCheckerPos) ? Color.red : Color.white;
        Gizmos.DrawLine(LeftCheckerPos, LeftCheckerPos + (Vector2)transform.right * CeilingCheckersDistance);

        Gizmos.color = CheckCeiling(RightCheckerPos) ? Color.red : Color.white;
        Gizmos.DrawLine(RightCheckerPos, RightCheckerPos + (Vector2)transform.right * CeilingCheckersDistance);

        Gizmos.color = CheckCeiling(ActualMainCeilingChecker) ? Color.red : Color.white;
        Gizmos.DrawWireCube(ActualMainCeilingChecker.center, ActualMainCeilingChecker.size);
    }

}
