using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapacheController : MonoBehaviour
{
    public int velocity = 4, velSalto = 5, salto = 3;
    public Vector3 rt;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;

    const int ANI_QUIETO = 0;
    const int ANI_CORRER = 1;
    const int ANI_SALTO = 1;
    int cont;
    Vector3 lastCheckpointPosition;
    
    void Start()
    {
        cont = salto;
        Debug.Log("Iniciando script de player");
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
    }

    void Update()
    {
        Movimientos();
    }
    void Movimientos(){
        if(Input.GetKey(KeyCode.RightArrow)){
            ChangeAnimation(ANI_CORRER);
            rb.velocity = new Vector2(velocity, rb.velocity.y);
            sr.flipX = false;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            ChangeAnimation(ANI_CORRER);
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
            sr.flipX = true;
        }else{
            rb.velocity = new Vector2(0, rb.velocity.y);
            ChangeAnimation(ANI_QUIETO);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(new Vector2(0, velSalto), ForceMode2D.Impulse);
            ChangeAnimation(ANI_SALTO);
            cont--;
        }
        
    }
    private void ChangeAnimation(int a){
        animator.SetInteger("Estado", a);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
         if(other.gameObject.tag=="piso")
        {
           
            Debug.Log("piso");        
        }
    }

}
