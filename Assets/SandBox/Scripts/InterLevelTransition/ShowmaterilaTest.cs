using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ShowmaterilaTest : MonoBehaviour
{
    Material a;
    [SerializeField] private Texture ffff;


    void Start()
    {
        a = GetComponent<MeshRenderer>().material;
      //  a.mainTexture = ffff;


    }

    // Update is called once per frame
    void Update()
    {
        print(a);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector2.right);
        print(raycastHit2D.distance);
        if (!raycastHit2D.collider.gameObject.CompareTag("Player")) ;
        Instantiate(gameObject, Vector3.zero, quaternion.identity);
        //   ray = new Vector2(V)

        //  RaycastHit2D
    }
}
