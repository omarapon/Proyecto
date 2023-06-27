using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PersonajeNivel7 : MonoBehaviour
{
 public int velocity = 4, veloCorrer = 8, velSalto = 5, salto = 2;
    //Audios
    public AudioClip jumpClip;
    public AudioClip deadClip;
    public AudioClip coinClip;
    public AudioClip AttackClip;
    AudioSource audioSource;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;
    Collider2D cl;
    
    
  
    float dir = 1.2f;
    float tiempoataque = 0.5f, time = 0;
    bool darGolpe = false;
    public GameObject golpe;

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

    private Nivel7Controller gameManager;

    void Start()
    {
        cont = salto;
        gameManager = FindObjectOfType<Nivel7Controller>();
        gameManager.LoadGame();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cl = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        if (gameManager.bonus)
        {
            transform.position = new Vector3(70, -13, 0);
            sr.flipX = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (ani == 0)
        {
            Movimientos();
        }
        if (Input.GetKeyDown("z"))
        {
            ChangeAnimation(ANI_ATAQUE);
            darGolpe = true;
        }
        if (darGolpe)
        {
            time += Time.deltaTime;
            if (time >= tiempoataque) Golpe();
        }
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
    
    void Muerte()
    {
        ani = 0;
        timeEnd = 0;
        rb.velocity = new Vector2(0, 0);
        ChangeAnimation(ANI_QUIETO);
    }
    void Golpe()
    {
        time = 0;
        audioSource.PlayOneShot(AttackClip);
        var golpePosition = transform.position + new Vector3(dir, -0.28f, 0);
        var gb = Instantiate(golpe, golpePosition, Quaternion.identity);
        var controller = gb.GetComponent<GolpeScript>();
        darGolpe = false;
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
        
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        
    }
}
