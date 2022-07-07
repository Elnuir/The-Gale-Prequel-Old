using System.Linq;
using UnityEngine;

public class NRedeerMovement : NPathMovementBase
{
    public Transform PlayerLeftShootingSpot;
    public Transform PlayerRightShootingSpot;

    private Rigidbody2D _physics;

    public bool isFacingRight;
    public float LookAtPlayerDistance = 5;
    public float MinNeighborDistance = 5;
    public LayerMask EnemiesLayer;

    protected override void Start()
    {
        base.Start();
        _physics = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        FlipFix();
        base.Update();
    }


    void FlipFix()
    {
        var playerObj = FindObjectOfType<Player>();
        Vector2 player;

        if (playerObj == null || Vector2.Distance(playerObj.transform.position, transform.position) > LookAtPlayerDistance)
            player = Destination;
        else
            player = playerObj.transform.position;

        bool localIsFacingRight = transform.localRotation.eulerAngles.y < 100;

        if (this.isFacingRight)
            localIsFacingRight = !localIsFacingRight;

        if (localIsFacingRight && player.x < transform.position.x)
        {
            transform.Rotate(0, 180, 0);
            localIsFacingRight = false;
        }

        if (!localIsFacingRight && player.x > transform.position.x)
        {
            transform.Rotate(0, 180, 0);
            localIsFacingRight = true;
        }
    }



    public override bool CanMoveTo(Vector3 position)
    {
        var objs = Physics2D.OverlapCircleAll(transform.position, MinNeighborDistance, EnemiesLayer);


        foreach (var obj in objs)
        {
            if (obj.transform == transform) continue;
            if (!obj.GetComponent<NRedeerMovement>()) continue;


            if (Vector2.Distance(transform.position, obj.transform.position) < MinNeighborDistance)
                if (Vector2.Distance(transform.position, Destination) > Vector2.Distance(obj.transform.position, obj.GetComponent<NRedeerMovement>().Destination))
                    // if (transform.GetInstanceID() < obj.transform.GetInstanceID() )
                    return false;
        }


        return true;
    }

    public override void MoveTo(Vector3 position)
    {
        var player = FindObjectOfType<Player>();
        if (player != null && Vector2.Distance(position, player.transform.position) < 0.5f)
            Destination = ChooseLeftOrRightSpot().position;
        else
            Destination = position;

        var delta = CurrentNode - transform.position;

        if (!IsPathNodeReached())
            _physics.velocity = delta.normalized * Speed;
    }

    public Transform ChooseLeftOrRightSpot()
    {
        float distanceL = Vector2.Distance(transform.position, PlayerLeftShootingSpot.position);
        float distanceR = Vector2.Distance(transform.position, PlayerRightShootingSpot.position);

        if (distanceL < distanceR)
            return PlayerLeftShootingSpot;
        return PlayerRightShootingSpot;
    }

    public override bool CanMoveToNow(Vector3 position)
    {
        return CanMoveTo(position);
    }
}