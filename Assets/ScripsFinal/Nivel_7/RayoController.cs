using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayoController : MonoBehaviour
{
    Rigidbody2D rb;
    public float velocity = 10;
    private Nivel7Controller gameManager;
    public void SetRightDirection()
    {
        velocity = 5;
    }
    public void SetLeftDirection()
    {
        velocity = -5;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D> ();
        gameManager = FindObjectOfType<Nivel7Controller>();
        Destroy(this.gameObject, 4);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velocity, 0);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name == "Boss"){

        }else Destroy(this.gameObject); //Se destruye la bala
        if (other.gameObject.tag == "Player")
        {
            gameManager.PerderVida();
        }
    }   
}
