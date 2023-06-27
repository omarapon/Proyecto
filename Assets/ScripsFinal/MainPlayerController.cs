using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour
{
    public int velocity = 2, velSalto = 5, salto = 3;
    public Vector3 rt;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;

    const int ANI_QUIETO_DER = 0;
    const int ANI_QUIETO_IZQ = 1;
    const int ANI_CORRER_DER = 2;
    const int ANI_CORRER_IZQ = 3;
    const int ANI_SALTO = 4;
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
            ChangeAnimation(ANI_CORRER_DER);
            rb.velocity = new Vector2(velocity, rb.velocity.y);
            sr.flipX = false;
        }
        else if(Input.GetKey(KeyCode.LeftArrow)){
            ChangeAnimation(ANI_CORRER_IZQ);
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
            sr.flipX = true;
        }else{
            rb.velocity = new Vector2(0, rb.velocity.y);
            if(sr.flipX) ChangeAnimation(ANI_QUIETO_IZQ);
            else ChangeAnimation(ANI_QUIETO_DER);
            
        }
        if(Input.GetKeyDown(KeyCode.Space) && cont>0){
            rb.AddForce(new Vector2(0, velSalto), ForceMode2D.Impulse);
            ChangeAnimation(ANI_SALTO);
            cont--;
        }
        
    }
    private void ChangeAnimation(int a){
        animator.SetInteger("Estado", a);
    }
}
