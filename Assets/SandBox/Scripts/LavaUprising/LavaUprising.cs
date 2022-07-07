using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaUprising : MonoBehaviour
{
    public float DamagePerSecond = 10;
    public float Speed = 1f;
    public float Interval = 0.1f;
    private float ReboundY = 0;
    private bool Rebounding;
    private Vector3 reboundVelocity;
    public GameObject portalPrefab;

    public Transform[] teleportPoints;
    public Queue<(GameObject, Transform)> spawnQueue = new Queue<(GameObject, Transform)>();

    private Camera Cumera;

    //
    // Start is called before the first frame update
    void Start()
    {
        Cumera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Rebounding)
        {

            if (ActionEx.CheckCooldown(Update, Interval))
                transform.position = new Vector3(transform.position.x, transform.position.y + Speed);
        }
        else
        {

            if (Mathf.Abs(transform.position.y - ReboundY) < 0.5f)
            {
                Rebounding = false;
                return;
            }

            var target = transform.position;
            target.y = ReboundY;
            transform.position = Vector3.SmoothDamp(transform.position, target, ref reboundVelocity, 0.3f);

        }
    }

    public void Rebound(float amount)
    {
        ReboundY = transform.position.y - amount;
        Rebounding = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && ActionEx.CheckCooldown((Action<Collider2D>)OnTriggerStay2D, 1f))
            other.SendMessage("Damage", new float[] { DamagePerSecond, 0 });

        if (other.CompareTag("Enemy"))
        {
            //    other.SendMessage("DamageReceive", new float[] { DamagePerSecond, 0 });
            Teleport(other.gameObject);
        }
    }

    private void Teleport(GameObject target)
    {
        var pointToTeleport = teleportPoints[UnityEngine.Random.Range(0, teleportPoints.Length)];

        portalPrefab.GetCloneFromPool(null, pointToTeleport.position, Quaternion.identity);
        target.SetActive(false);

        spawnQueue.Enqueue((target, pointToTeleport.transform));
        Invoke(nameof(RespawnNextTarget), 1.5f);
    }

    // moves target to up spot and enables it 
    private void RespawnNextTarget()
    {
        if (spawnQueue.Count > 0)
        {
            var (target, teleportTo) = spawnQueue.Dequeue();
            target.transform.position = teleportTo.position;
            target.SetActive(true);
        }
    }

}
