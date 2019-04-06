using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.position.y > transform.position.y)
            GetComponent<EdgeCollider2D>().isTrigger = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.transform.position.y < transform.position.y)
            GetComponent<EdgeCollider2D>().isTrigger = true;
    }
}
