using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cherryspawn : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Object.Destroy(gameObject);
        collision.gameObject.GetComponent<PlayerController>().otherplayer.gameObject.GetComponent<PlayerController>().score = 0;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        System.Random random = new System.Random();
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(random.Next(-10, 10), random.Next(-10, 10));
    }
}
