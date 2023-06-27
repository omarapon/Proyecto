using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PersonajeNivel3 : MonoBehaviour
{
    public int velocity = 8, velSalto = 5, salto = 3;    //Audios
    public AudioClip jumpClip;
    public AudioClip deadClip;
    AudioSource audioSource;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    public GameObject humo;


    const int ANI_QUIETO = 0;
    const int ANI_CAMINAR = 1;
    const int ANI_SALTO = 3;
    const int ANI_ATAQUE = 5;
    const int ANI_TREPAR = 6;
    const int ANI_DANIO = 7;
    const int ANI_MUERTO = 8;

    const int ANI_JETPACK = 9;
    const int ANI_OTHER = 12;

    int ani = 0, cont;
    float timemuerto = 1.5f, timeEnd = 0;
    Vector3 lastCheckpointPosition;
    private Nivel3Controller gameManager;

    void Start()
    {
        cont = salto;
        Debug.Log("Iniciando script de player");
        gameManager = FindObjectOfType<Nivel3Controller>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
        gameManager.LoadGame();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ani == 0)
        {
            Movimientos();
        }
        else if (ani == 1)
        {
            Libre();
        }
        else if (ani == 2)
        {
            if (gameManager.lives <= 0)
            {
                ChangeAnimation(ANI_MUERTO);
            }
            else
            {
                ChangeAnimation(ANI_DANIO);
            }
            gameManager.inicio = false;
            rb.velocity = new Vector2(0, 2);
            timeEnd += Time.deltaTime;
            if (timeEnd >= timemuerto) Muerte();
        }
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
            gameManager.inicio = false;
            rb.velocity = new Vector2(0, 0);
            timeEnd += Time.deltaTime;
            if (timeEnd >= timemuerto) Muerte();
        }

    }

    void Muerte()
    {
        ani = 0;
        timeEnd = 0;
        gameManager.inicio = false;
        gameManager.muerto = true;
        rb.velocity = new Vector2(0, 0);
        ChangeAnimation(ANI_QUIETO);
        if (gameManager.check == 0) transform.position = new Vector3(-9.5f, -2, 0);
        else if (gameManager.check == 1) transform.position = new Vector3(135.5f, -2, 0);
        else if (gameManager.check == 2) transform.position = new Vector3(212f, -2, 0);
    }

    void QuitarJetPack()
    {
        ani = 0;
        ChangeAnimation(ANI_QUIETO);
        gameManager.inicio = false;
    }
    void Movimientos()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = false;
            ChangeAnimation(ANI_CAMINAR);
            rb.velocity = new Vector2(velocity, rb.velocity.y);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            sr.flipX = true;
            ChangeAnimation(ANI_CAMINAR);
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
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
    void Libre()
    {
        ChangeAnimation(ANI_JETPACK);
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            sr.flipX = false;
            rb.velocity = new Vector2(velocity, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-velocity, rb.velocity.y);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, velSalto), ForceMode2D.Impulse);

            var humoPosition = transform.position + new Vector3(-0.4f, -1.5f, 0);
            var gb = Instantiate(humo, humoPosition, Quaternion.identity);
        }


    }

    void OnCollisionEnter2D(Collision2D other)
    {
        cont = salto;

        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Limites")
        {
            audioSource.PlayOneShot(deadClip);
            if (other.gameObject.name == "Abajo") ani = 2;
            else ani = 3;
            gameManager.PerderVida();
        }
        if (other.gameObject.tag == "CheckPoint")
        {
            Debug.Log("CheckPoint");
            QuitarJetPack();
            Destroy(other.gameObject);
            if (other.gameObject.name == "Flag") gameManager.check = 1;
            else if (other.gameObject.name == "Flag2")
            {
                
                gameManager.check = 2;
            }
        }
        if (other.gameObject.tag == "TP")
        {
            SceneManager.LoadScene(3);  //Volver al nivel regular
        }
    }
    void OnTriggerEnter2D(Collider2D other)//para reconocer el checkponit(transparente)
    {
        if (other.gameObject.tag == "JetPack")
        {
            ani = 1;
            gameManager.inicio = true;
            other.GetComponent<Collider2D>().enabled = false;
            other.GetComponent<SpriteRenderer>().enabled = false;
            //Destroy(other.gameObject);
        }
    }
    private void ChangeAnimation(int a)
    {
        animator.SetInteger("Estado", a);
    }
}
