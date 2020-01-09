using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //float paddleMinY = -3.6f; // how far we want to be padddle to travel
    //float paddleMaxY = 3.6f;
    float paddleMaxSpeed = 5;

    void Start()
    {
    }

    void Update()
    {
        float translation = Input.GetAxis("Vertical") * paddleMaxSpeed;
        translation *= Time.deltaTime;
        transform.Translate(0, translation, 0);
    }
}
