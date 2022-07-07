using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltTargetProvider : TargetProviderBase
{
    EnemyAIPathAndMoveBlackMatter enemyAIPathAndMoveBlackMatter;
    [SerializeField] private ChaserBlackMatter chaserBlackMatter;
    private Rigidbody2D rigidbody2D;
    private GameObject marker;

    private void Start()
    {
        rigidbody2D = GetComponentInParent<Rigidbody2D>();
        marker = new GameObject();
        enemyAIPathAndMoveBlackMatter = GetComponentInParent<EnemyAIPathAndMoveBlackMatter>();
    }

    public override Transform GetTarget()
    {
        if (!chaserBlackMatter.isImpacting)
        {
            return enemyAIPathAndMoveBlackMatter.target;
        }
        else
        {
            if (rigidbody2D.velocity.x > 0.01f)
                marker.transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            else if (rigidbody2D.velocity.x < -0.01f)
                marker.transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            else if (rigidbody2D.velocity.y < -0.01f)
                marker.transform.position = new Vector2(transform.position.x, transform.position.y);
            return marker.transform;
        }
    }
}