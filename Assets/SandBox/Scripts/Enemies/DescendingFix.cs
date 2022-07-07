using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEngine;

public class DescendingFix : MonoBehaviour
{
    private int MinDeltaX = 3500;
    private int MaxDeltaY = 3000;
    
    private Seeker PathSeeker;

    public bool IsGoingDown
    {
        get
        {
            var nextWp = PathSeeker.GetCurrentPath().path.Last();
            var currPos = PathSeeker.GetCurrentPath().path[0];
            
            if(Mathf.Abs(nextWp.position.x - currPos.position.x) <= MinDeltaX)
                if (Mathf.Abs(currPos.position.y - nextWp.position.y) >= MaxDeltaY)
                    return true;
            
//            Debug.Log(nextWp.position.x - currPos.position.x < 500);
            return false;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        PathSeeker = GetComponent<Seeker>();
    }

    // Update is called once per frame
    void Update()
    {
       var a= IsGoingDown;

       if (a)
       {
           if (!GetComponent<Nibbler>().isIdling)
               GetComponent<Nibbler>().isIdling = true;
           
           if (GetComponent<Nibbler>().isMoving)
               GetComponent<Nibbler>().isMoving = false;
           

           GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
        
         
       }
       else
       {
           if (GetComponent<Nibbler>().isIdling)
               GetComponent<Nibbler>().isIdling = false;
           
           if (!GetComponent<Nibbler>().isMoving)
               GetComponent<Nibbler>().isMoving = true;
           

       }

       //   GoSomewhere();
    }

    void GoSomewhere()
    {
        var nextWp = PathSeeker.GetCurrentPath().path[2];
        var currPos = PathSeeker.GetCurrentPath().path[0];

        if (nextWp.position.x > currPos.position.x)
            GetComponent<Rigidbody2D>().velocity = new Vector2(2, 0);
        else
            GetComponent<Rigidbody2D>().velocity = new Vector2(-2, 0);
    }
    
}
