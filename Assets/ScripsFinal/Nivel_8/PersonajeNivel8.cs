using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PersonajeNivel8 : MonoBehaviour
{
    public int velocity = 4, veloCorrer = 8, velSalto = 5, salto = 2;
    //Audios
    public AudioClip jumpClip;
    public AudioClip deadClip;
    public AudioClip coinClip;
    AudioSource audioSource;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    public TilemapCollider2D plataform;
    public GameObject irBonus;
    public GameObject Camara;

    public float climbSpeed = 3;
    public float exitHop = 3;
    [HideInInspector] public bool onLadder = false;
    [HideInInspector] public bool isGrounded = true;
    [HideInInspector] public bool usingLadder = false;

    const int ANI_QUIETO = 0;
    const int ANI_CAMINAR = 1;
    const int ANI_CORRER = 2;
    const int ANI_SALTO = 3;
    const int ANI_SLIDE = 4;
    const int ANI_ATAQUE = 5;
    const int ANI_TREPAR = 6;
    const int ANI_DANIO = 7;
    const int ANI_MUERTO = 8;

    int ani = 0, cont;
    float timemuerto = 1.5f, timeEnd = 0;
    Vector3 lastCheckpointPosition = new Vector3(-77, -7, 0);

    private Nivel8Controller gameManager;

    void Start()
    {
        cont = salto;
        gameManager = FindObjectOfType<Nivel8Controller>();
        gameManager.LoadGame();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        Camara.transform.position = new Vector3(-68, -4, -10);
        if (gameManager.bonus)
        {
            irBonus.GetComponent<Collider2D>().enabled = false;
            irBonus.GetComponent<SpriteRenderer>().enabled = false;
            transform.position = new Vector3(70, -13, 0);
            sr.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoverCamara();

        if (ani == 0)
        {
            Movimientos();
        }/*
        else if (ani == 3)
        {
            if (gameManager.lives <= 0)
            {
                ChangeAnimation(ANI_MUERTO);
            }
            else
            {
                ChangeAnimation(ANI_DANIO);
            }
            rb.velocity = new Vector2(0, 0);
            Debug.Log(timeEnd);
            timeEnd += Time.deltaTime;
            if (timeEnd >= timemuerto) Muerte();
        }*/
        if (Input.GetKey("r"))
        {
            gameManager.ReiniciarSave();
            gameManager.LoadGame();
        }
        if (Input.GetKey("t"))
        {
            gameManager.PonerMonedas();
            gameManager.LoadGame();
        }
        if(gameManager.saltoTriple>0){
            salto = 3;
        }
        else salto = 2;
    }
    void MoverCamara()
    {
        if (transform.position.y >= -10)
        {
            if (transform.position.y <= 5)
            {
                if (transform.position.y <= -5)
                {
                    Camara.transform.position = new Vector3(Camara.transform.position.x, -5, -10);
                }
                else Camara.transform.position = new Vector3(Camara.transform.position.x, transform.position.y, -10);
            }
            else
            {
                Camara.transform.position = new Vector3(Camara.transform.position.x, 5, -10);
            }
        }
        else
        {
            if (transform.position.y <= -15)
            {
                if (transform.position.y <= -23)
                {
                    Camara.transform.position = new Vector3(Camara.transform.position.x, -23, -10);
                }
                else Camara.transform.position = new Vector3(Camara.transform.position.x, transform.position.y, -10);
            }
            else
            {
                Camara.transform.position = new Vector3(Camara.transform.position.x, -15, -10);
            }
        }
        if (transform.position.x <= 72.5f)
        {
            if (transform.position.x <= -71)
            {
                Camara.transform.position = new Vector3(-71, Camara.transform.position.y, -10);
            }
            else Camara.transform.position = new Vector3(transform.position.x, Camara.transform.position.y, -10);
        }
        else
        {
            Camara.transform.position = new Vector3(72.5f, Camara.transform.position.y, -10);
        }
        //Camara.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }
    void Muerte()
    {
        ani = 0;
        timeEnd = 0;
        rb.velocity = new Vector2(0, 0);
        ChangeAnimation(ANI_QUIETO);
    }

    void Movimientos()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = false;

            if (Input.GetKey("x"))
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    ChangeAnimation(ANI_SLIDE);
                    rb.velocity = new Vector2(veloCorrer, rb.velocity.y);
                }
                else
                {
                    ChangeAnimation(ANI_CORRER);
                    rb.velocity = new Vector2(veloCorrer, rb.velocity.y);
                }
            }
            else
            {
                ChangeAnimation(ANI_CAMINAR);
                rb.velocity = new Vector2(velocity, rb.velocity.y);
            }

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            sr.flipX = true;

            if (Input.GetKey("x"))
            {
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    ChangeAnimation(ANI_SLIDE);
                    rb.velocity = new Vector2(-veloCorrer, rb.velocity.y);
                }
                else
                {
                    ChangeAnimation(ANI_CORRER);
                    rb.velocity = new Vector2(-veloCorrer, rb.velocity.y);
                }
            }
            else
            {
                ChangeAnimation(ANI_CAMINAR);
                rb.velocity = new Vector2(-velocity, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            ChangeAnimation(ANI_QUIETO);
        }
        if (Input.GetKeyDown(KeyCode.Space) && cont > 0)
        {
            audioSource.PlayOneShot(jumpClip);
            cont--;
            if(gameManager.saltoTriple > 0 && cont == 0){
                gameManager.MenosSaltos();
            }
            rb.AddForce(new Vector2(0, velSalto), ForceMode2D.Impulse);
            ChangeAnimation(ANI_SALTO);
        }
        if(onLadder) ChangeAnimation(ANI_TREPAR);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        cont = salto;
        isGrounded = true;

        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Limites")
        {
            ani = 0;
            audioSource.PlayOneShot(deadClip);
            if (lastCheckpointPosition != null)
            {
                transform.position = lastCheckpointPosition;
            }
            gameManager.PerderVida();
        }
        if (other.gameObject.tag == "Moneda")
        {
            audioSource.PlayOneShot(coinClip);
            gameManager.GanarPuntos(1);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Vida")
        {
            gameManager.GanarVida();
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Triple")
        {
            gameManager.MasSaltos();
            gameManager.PrintSaltoInScreen();
            cont = 3;
            Destroy(other.gameObject);
        }
        if (other.gameObject.name == "Shield")
        {
            gameManager.bonus = true;
            gameManager.SaveGame();
            SceneManager.LoadScene(3);  //Cambia al Nivel Bonus
            lastCheckpointPosition = transform.position;
            irBonus.GetComponent<Collider2D>().enabled = false;
            irBonus.GetComponent<SpriteRenderer>().enabled = false;
        }
        if (other.gameObject.tag == "TP")
        {
            transform.position = new Vector3(43, -25.5f, 0);
        }
        if (other.gameObject.tag == "JetPack")
        {
            gameManager.SaveGame();
            SceneManager.LoadScene(2);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "CheckPoint")
        {
            Debug.Log("CheckPoint");//aplicar la pocion isTrigger en la configuracion
            lastCheckpointPosition = transform.position;
            other.GetComponent<Collider2D>().enabled = false;
        }
    }
    private void ChangeAnimation(int a)
    {
        animator.SetInteger("Estado", a);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Escalera"))
        {
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * climbSpeed);
                rb.gravityScale = 0;
                onLadder = true;
                plataform.enabled = false;
            }
            else if (Input.GetAxisRaw("Vertical") == 0 && onLadder)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Escalera") && onLadder)
        {
            rb.gravityScale = 1;
            onLadder = false;
            plataform.enabled = true;

            if (!isGrounded)
                rb.velocity = new Vector2(rb.velocity.x, exitHop);
        }
    }
}
