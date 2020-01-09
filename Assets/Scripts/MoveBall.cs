using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveBall : MonoBehaviour
{
    Vector3 ballStartPosition;
    Rigidbody2D rb;
    float speed = 600;
    public AudioSource blip;
    public AudioSource blop;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        ballStartPosition = this.transform.position;
        ResetBall(); // to restart a ball
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "backwall")
            blop.Play();
        else
            blip.Play();
    }

    private void ResetBall()
    {
        this.transform.position = ballStartPosition;
        rb.velocity = Vector3.zero;
        Vector3 dir = new Vector3(UnityEngine.Random.Range(100, 300), UnityEngine.Random.Range(-100, 100), 0).normalized;
        rb.AddForce(dir * speed);
    }


    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ResetBall();
        }
    }
}
