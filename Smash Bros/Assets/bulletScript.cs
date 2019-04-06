﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject launchedObject;
    public int damage;
    public int speed;
    public int kb;
    public int direction;
    // Update is called once per frame 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "player" && !collision.collider.gameObject.Equals( launchedObject ))
        {
            Debug.Log("YO");
            collision.collider.GetComponent<PlayerController>().knockedBack = true;
            collision.collider.GetComponent<PlayerController>().knockSpeed = direction*kb;
            collision.collider.GetComponent<PlayerController>().otherplayer.GetComponent<PlayerController>().score += 30;
            //Object.Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        rb.velocity = new Vector2(speed*direction, 0);
    }
}
