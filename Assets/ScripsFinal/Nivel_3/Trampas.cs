using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampas : MonoBehaviour
{
    Rigidbody2D rb;
    int dir = -2;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, dir);

        if (transform.position.y >= 4.2f) dir = -2;
        else if (transform.position.y <= -3.4f) dir = 2;

    }
}
