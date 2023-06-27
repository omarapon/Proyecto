using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Nivel8Controller : MonoBehaviour
{

    public Text scoreText;
    public Text livesText;
    public Text saltosText;
    private int score = 0;
    public int lives = 3;
    public bool bonus = false;
    public int saltoTriple = 0;

    void Start()
    {

    }
    void Update()
    {
        if (lives < 0)
        {
            SceneManager.LoadScene(0);
            ReiniciarSave();
        }
    }

    public void SaveGame()
    {
        var filePath = Application.persistentDataPath + "/guardar.dat";
        FileStream file;

        Debug.Log("File.Exists(filePath)"+ File.Exists(filePath)    );

        if (File.Exists(filePath))
            file = File.OpenWrite(filePath);
        else
            file = File.Create(filePath);

        GameData data = new GameData();
        data.Score = score;
        data.Live = lives;
        data.Bonus = bonus;
        data.SaltoTriple = saltoTriple;

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
        bonus = data.Bonus;
        saltoTriple = data.SaltoTriple;

        GanarPuntos(0);
    }
    public void PonerMonedas()
    {
        var filePath = Application.persistentDataPath + "/guardar.dat";
        FileStream file;

        if (File.Exists(filePath))
            file = File.OpenWrite(filePath);
        else
            file = File.Create(filePath);

        GameData data = new GameData();
        data.Score = 50;
        data.Live = 3;
        data.Bonus = false;

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Reiniciado");
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
        data.Bonus = false;
        data.SaltoTriple = 0;

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
        PrintSaltoInScreen();
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
    public void MasSaltos()
    {
        saltoTriple ++;
        PrintSaltoInScreen();
    }
    public void MenosSaltos()
    {
        saltoTriple --;
        PrintSaltoInScreen();
    }
    public void PrintScoreInScreen()
    {
        scoreText.text = "Puntaje: " + score;
    }
    public void PrintLivesInScreen()
    {
        livesText.text = "Vidas: " + lives;
    }
    public void PrintSaltoInScreen()
    {
        saltosText.text = "Saltos Triples: " + saltoTriple;
    }
}