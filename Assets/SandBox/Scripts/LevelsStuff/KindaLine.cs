using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class KindaLine : MonoBehaviour
{
    public Transform From;
    public Transform To;
    public float DestroyAfter;
    public float MagicNumber = 0.2f;
    public float DistanceLimit = 100f;

    void Start()
    {
        Invoke(nameof(SelfDestruct), DestroyAfter);
    }

    void Update()
    {
        if (Vector2.Distance(From.transform.position, To.transform.position) <= DistanceLimit)
            Redraw();
    }

    void Redraw()
    {
        transform.right = To.position - transform.position;
        float distance = Vector2.Distance(From.position, To.position);
        transform.localScale = new Vector3(distance * MagicNumber, transform.localScale.y, transform.localScale.z);
    }

    void SelfDestruct()
    {
        GameObject.Destroy(gameObject);
    }
}
