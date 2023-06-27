using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraScroll : MonoBehaviour
{
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        rb.velocity = new Vector2(Nivel3Controller.instance.scrollSpeed, 0);
        //rb.velocity = Vector2.left * Nivel3Controller.instance.scrollSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Nivel3Controller.instance.gameOver) rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(Nivel3Controller.instance.scrollSpeed, 0);
    }
}
