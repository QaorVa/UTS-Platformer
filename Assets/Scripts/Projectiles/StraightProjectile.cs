using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectile : MonoBehaviour
{
    
    public float speed = 5f;
    public float maxSpeed = 10f;

    
    public float acceleration = 2f;

    // Update is called once per frame
    void Update()
    {
        if(speed >= maxSpeed)
        {
            speed = maxSpeed;
        } else
        {
            speed += acceleration * Time.deltaTime;
        }
 
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
