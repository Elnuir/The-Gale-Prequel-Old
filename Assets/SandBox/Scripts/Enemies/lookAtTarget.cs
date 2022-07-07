using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtTarget : MonoBehaviour
{
    private EnemyAIPathAndMoveBlackMatter targetToLook;
    private Transform target;
    [SerializeField] float speedRotation;
    private Vector3 reference;
    void Start()
    {
        targetToLook = GetComponentInParent<EnemyAIPathAndMoveBlackMatter>();
        
    }

    // Update is called once per frame
    void Update()
    {
        target = targetToLook.target;
        transform.right = Vector3.SmoothDamp(target.position, target.position - transform.position, ref reference, 0.4f);
      //  target = targetToLook.target;
        //transform.rotation = new Vector2(transform.rotation.x, transform.rotation.y, )  transform.LookAt(target);
        // Vector3 targetDirecton = target.transform.position - transform.position;
        // targetDirecton.y = 0.0f;
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDirecton),
        //     Time.time * speedRotation);
       // Quaternion.RotateTowards(transform.position, target.rotation, Time.time * speedRotation);
    }
}
