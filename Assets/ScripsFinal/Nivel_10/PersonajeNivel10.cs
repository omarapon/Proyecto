using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PersonajeNivel10 : MonoBehaviour
{
    public int velocity = 4, veloCorrer = 8, velSalto = 5, salto = 2;
    //Audios
    public AudioClip jumpClip;
    public AudioClip coinClip;
    AudioSource audioSource;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    public GameObject Camara;

    const int ANI_QUIETO = 0;
    const int ANI_CAMINAR = 1;
    const int ANI_CORRER = 2;
    const int ANI_SALTO = 3;
    const int ANI_SLIDE = 4;
    const int ANI_ATAQUE = 5;
    const int ANI_TREPAR = 6;
    const int ANI_DANIO = 7;
    const int ANI_MUERTO = 8;

    int cont;
    Vector3 lastCheckpointPosition = new Vector3(-77, -7, 0);

    private Nivel10Controller gameManager;

    void Start()
    {
        cont = salto;
        gameManager = FindObjectOfType<Nivel10Controller>();
        gameManager.LoadGame();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        Camara.transform.position = new Vector3(-4.5f, -4, -10);
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.inicio)
        {
            MoverCamara();
            Movimientos();  
            if (Input.GetKey("r"))
            {
                gameManager.ReiniciarSave();
                gameManager.LoadGame();
            }
        }
        else
        {
            if (transform.position.x >= -7.5f) gameManager.inicio = true;
            ChangeAnimation(ANI_CAMINAR);
            rb.velocity = new Vector2(1.8f, rb.velocity.y);
        }


    }
    void MoverCamara()
    {
        if (transform.position.y <= 4.5f)
        {
            if (transform.position.y <= -4)
            {
                Camara.transform.position = new Vector3(Camara.transform.position.x, -4, -10);
            }
            else Camara.transform.position = new Vector3(Camara.transform.position.x, transform.position.y, -10);
        }
        else
        {
            Camara.transform.position = new Vector3(Camara.transform.position.x, 4.5f, -10);
        }
        if (transform.position.x <= 14)
        {
            if (transform.position.x <= -7f)
            {
                Camara.transform.position = new Vector3(-7f, Camara.transform.position.y, -10);
            }
            else Camara.transform.position = new Vector3(transform.position.x, Camara.transform.position.y, -10);
        }
        else
        {
            Camara.transform.position = new Vector3(14, Camara.transform.position.y, -10);
        }
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
            rb.AddForce(new Vector2(0, velSalto), ForceMode2D.Impulse);
            ChangeAnimation(ANI_SALTO);
            cont--;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        cont = salto;

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
        if (other.gameObject.tag == "TP")
        {
            SceneManager.LoadScene(5);  //Cambia al Nivel Bonus
            gameManager.SaveGame();
            Debug.Log("Ganaste las monedas");
        }
    }
    private void ChangeAnimation(int a)
    {
        animator.SetInteger("Estado", a);
    }

}
