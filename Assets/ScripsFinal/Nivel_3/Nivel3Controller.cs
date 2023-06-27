using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Nivel3Controller : MonoBehaviour
{
    public static Nivel3Controller instance;
    public Text scoreText, livesText;
    public int score = 0, lives = 3;
    public float scrollSpeed = 0, PerX = 0;
    public bool inicio = false, muerto = false, gameOver = false;
    public int check = 0;
    public GameObject Limite;
    public GameObject Camara;
    public GameObject jet;
    public GameObject jet2; 

    private void Awake()
    {
        if (Nivel3Controller.instance == null)
        {
            Nivel3Controller.instance = this;
        }
        else if (Nivel3Controller.instance != this)
        {
            Destroy(gameObject);
            Debug.LogWarning("GameController ha sido instanciado por segunda vez. Esto no deberia pasar");
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lives<0)
        {
            SceneManager.LoadScene(0);
            ReiniciarSave();
        }
        if (inicio) scrollSpeed = 4f;
        else scrollSpeed = 0;
        if (muerto)
        {
            if (check == 0)
            {
                Limite.transform.position = new Vector3(-13.6f, 0.5f, 0);
                Camara.transform.position = new Vector3(-3, 0, -10);
            }
            else if (check == 1)
            {
                Limite.transform.position = new Vector3(126.4f, 0.5f, 0);
                Camara.transform.position = new Vector3(140, 0, -10);
            }
            else if (check == 2)
            {
                Limite.transform.position = new Vector3(126.4f, 0.5f, 0);
                Camara.transform.position = new Vector3(215, 0, -10);
            }
            if (check == 2)
            {
                Limite.transform.position = new Vector3(126.4f, 0.5f, 0);
                Camara.transform.position = new Vector3(215, 0, -10);
            }

            muerto = false;
            jet.GetComponent<Collider2D>().enabled = true;
            jet.GetComponent<SpriteRenderer>().enabled = true;
            jet2.GetComponent<Collider2D>().enabled = true;
            jet2.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    public void SaveGame()
    {
        var filePath = Application.persistentDataPath + "/guardar.dat";
        FileStream file;

        Debug.Log("File.Exists(filePath)" + File.Exists(filePath));

        if (File.Exists(filePath))
            file = File.OpenWrite(filePath);
        else
            file = File.Create(filePath);

        GameData data = new GameData();
        data.Score = score;
        data.Live = lives;
        data.Bonus = false;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
    }
    public void LoadGame()
    {
        var filePath = Application.persistentDataPath + "/guardar.dat";
        FileStream file;

        if (File.Exists(filePath))
            file = File.OpenRead(filePath);
        else
        {
            Debug.LogError("No see encontro archivo");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(file);
        file.Close();

        //usar datos guardados
        score = data.Score;
        lives = data.Live;

        GanarPuntos(0);
    }
    public void ReiniciarSave()
    {
        var filePath = Application.persistentDataPath + "/guardar.dat";
        FileStream file;

        if (File.Exists(filePath))
            file = File.OpenWrite(filePath);
        else
            file = File.Create(filePath);

        GameData data = new GameData();
        data.Score = 0;
        data.Live = 3;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Reiniciado");
    }
    public void GanarPuntos(int puntos)
    {
        score += puntos;
        PrintLivesInScreen();
        PrintScoreInScreen();
    }
    public void PerderVida()
    {
        lives -= 1;
        PrintLivesInScreen();
    }
    public void GanarVida()
    {
        lives += 1;
        PrintLivesInScreen();
    }
    public void PrintScoreInScreen()
    {
        scoreText.text = "Puntaje: " + score;
    }
    public void PrintLivesInScreen()
    {
        livesText.text = "Vidas: " + lives;
    }

}

