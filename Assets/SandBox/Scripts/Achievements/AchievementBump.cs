using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementBump : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    [SerializeField] private float speed;

    [SerializeField] float timeMoving;
    private string achievementName;
    private string pattern = "Achievement: ";
    private Text text;
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        text = GetComponent<Text>();
        achievementName = text.text;
        text.text = pattern + achievementName;
    }

    // Update is called once per frame
    void Update()
    {
        timeMoving -= Time.deltaTime;
        if (timeMoving - Time.deltaTime >= 0)
        {
            rigidbody2D.AddForce(Vector2.up * speed * Time.deltaTime);
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
            Invoke(nameof(DisableObject), 3f);
        }
        
    }

    void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
