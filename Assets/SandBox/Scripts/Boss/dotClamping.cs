using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotClamping : MonoBehaviour
{
    [SerializeField]public bool isClampedX, isClampedY;
  //  [SerializeField] private float minX, maxX, minY, maxY;
   // private float baseX, baseY;
    private Player player;
    private float speed;
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void Update()
    {
      if(isClampedY)
      transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
      else if(isClampedX) 
          transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
    }
}
