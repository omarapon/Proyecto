using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNivel7 : MonoBehaviour
{
    int vida = 2;
    const int ANI_QUIETO = 0;
    const int ANI_ATAQUE = 1;
    const int ANI_SPECIAL = 2;
    const int ANI_SPECIAL1 = 3;
    const int ANI_HURT = 4;
    const int ANI_MUERTO = 5;
    const int ANI_CORRER = 6;
    public GameObject rayo;
    AudioSource audioSource;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    private float dir = -1.2f;
    private float timer = 0f;
    private float intervalo = 4.0f;
    public Transform target;  // Referencia al objeto del personaje
    public float speed = 5f;  // Velocidad de movimiento del enemigo
    public float detectionRange = 5f;  // Rango de detección del enemigo
    public GameObject portal;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        // Buscar el objeto del personaje por su etiqueta (asegúrate de que el personaje tiene la etiqueta correspondiente)
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(vida==0) {
            ChangeAnimation(ANI_MUERTO);
            cl.enabled = false;
            rb.isKinematic = true;
            portal.SetActive(!portal.activeSelf);
        }
        else mov();
        
    }
    private void mov(){
        // Calcular la distancia entre el enemigo y el objetivo
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // Comprobar si el objetivo está dentro del rango de detección del enemigo
        if (distanceToTarget <= detectionRange)
        {
            // Calcular la dirección hacia la cual debe moverse el enemigo
            Vector3 direction = (target.position - transform.position).normalized;

            // Mover el enemigo hacia la dirección calculada
            transform.position += direction * speed * Time.deltaTime;
            ChangeAnimation(ANI_CORRER);

            // Determinar si está yendo a la izquierda o a la derecha
            if (direction.x > 0)
            {
                dir = 1.2f;
                sr.flipX = false;
            }
            else if (direction.x < 0)
            {
                dir = -1.2f;
                sr.flipX = true;
            }
            else
            {
                
            }
        } 
        else if(distanceToTarget <= 0.5f){
            ChangeAnimation(ANI_ATAQUE);
        }

        else {
            // Incrementar el temporizador
            timer += Time.deltaTime;

            // Verificar si ha pasado el intervalo de tiempo
            if (timer >= intervalo){
                var rayoPosition = transform.position + new Vector3(dir, 0.6f, 0);
                var gb = Instantiate(rayo, rayoPosition, Quaternion.identity);
                var controller = gb.GetComponent<RayoController>();
                if (dir == 1.2f) controller.SetRightDirection();
                else controller.SetLeftDirection();
                ChangeAnimation(ANI_QUIETO);
                timer = 0f;
                
            }
            else if (timer >= 3.0f && timer <=3.2f){
                ChangeAnimation(ANI_SPECIAL1);
            }

            
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            vida= vida-1;
            Debug.Log("si");
        }
    }

    private void ChangeAnimation(int a)
    {
        animator.SetInteger("Estado", a);
    }
}
