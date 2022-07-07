using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CornerAntihit : MonoBehaviour
{
    public float CeilingCheckersInterval;
    public float CeilingCheckersDistance;
    public Vector2 CeilingCheckersOffset;
    public Rect MainCeilingChecker = new Rect(0, 1, 1, 1);
    public LayerMask WhatIsCeiling;

    private Vector2 ActualCenter => (Vector2)transform.position + CeilingCheckersOffset * Mathf.Sign(transform.right.x);
    private Vector2 LeftCheckerPos => ActualCenter - new Vector2(CeilingCheckersInterval / 2, 0);
    private Vector2 RightCheckerPos => ActualCenter + new Vector2(CeilingCheckersInterval / 2, 0);
    private Rect ActualMainCeilingChecker => new Rect((Vector2)transform.position + MainCeilingChecker.position, MainCeilingChecker.size);
    public bool DrawGizmos = true;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsRightTeleportRequired() && ActionEx.CheckCooldown(Update, 0.4f))
            RightTeleport();

        else if (IsLeftTeleportRequired() &&ActionEx.CheckCooldown(Update, 0.4f) )
            LeftTeleport();
    }

    private void RightTeleport()
    {
        transform.position = new Vector2(RightCheckerPos.x, transform.position.y);
    }

    private void LeftTeleport()
    {
        transform.position = new Vector2(LeftCheckerPos.x, transform.position.y);
    }

    private bool IsRightTeleportRequired()
    {
        if (_player.isGrounded) return false;
        return CheckCeiling(ActualMainCeilingChecker) && CheckCeiling(LeftCheckerPos) && !CheckCeiling(RightCheckerPos);
    }

    private bool IsLeftTeleportRequired()
    {
        if (_player.isGrounded) return false;
        return CheckCeiling(ActualMainCeilingChecker) && !CheckCeiling(LeftCheckerPos) && CheckCeiling(RightCheckerPos);
    }



    private bool CheckCeiling(Vector2 start)
    {
        return Physics2D.Raycast(start, transform.up, CeilingCheckersDistance, WhatIsCeiling);
    }


    private bool CheckCeiling(Rect area)
    {
        return Physics2D.OverlapArea(area.min, area.max, WhatIsCeiling);
    }

    private void OnDrawGizmos()
    {
        if (!DrawGizmos) return;
        Gizmos.color = CheckCeiling(LeftCheckerPos) ? Color.red : Color.white;
        Gizmos.DrawLine(LeftCheckerPos, LeftCheckerPos + (Vector2)transform.up * CeilingCheckersDistance);

        Gizmos.color = CheckCeiling(RightCheckerPos) ? Color.red : Color.white;
        Gizmos.DrawLine(RightCheckerPos, RightCheckerPos + (Vector2)transform.up * CeilingCheckersDistance);

        Gizmos.color = CheckCeiling(ActualMainCeilingChecker) ? Color.red : Color.white;
        Gizmos.DrawWireCube(ActualMainCeilingChecker.center, ActualMainCeilingChecker.size);
    }
}
