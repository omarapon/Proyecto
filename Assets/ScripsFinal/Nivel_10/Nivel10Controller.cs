using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Nivel10Controller : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;
    public Text timeText;
    private int score = 0;
    private int lives = 3;
    public float countdown = 40.0f;
    public bool inicio = false;
    public bool bonus = false;
    private int StarNivel1 = 0;
    private int StarNivel2 = 0;
    private int StarNivel3 = 0;
    private int StarNivel4 = 0;

    void Start()
    {
        
    }
    void Update()
    {
        PrintTimeInScreen();
        if (inicio)
        {
            if (countdown >= 0.0f) countdown -= Time.deltaTime;
            else
            {
                SceneManager.LoadScene(5);  
                Debug.Log("Perdiste");
            }
        }

    }
    public void Estrellas(){
        if(countdown>=15) StarNivel4 = 3;
        else if(countdown>=10) StarNivel4 = 2;
        else if(countdown>=5) StarNivel4 = 1;
        else StarNivel4=0;
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
        Estrellas();
        Debug.Log(StarNivel1 + "-" + StarNivel2 + "-" + StarNivel3 + "-" + StarNivel4);
        data.Nivel1Star=StarNivel1;
        data.Nivel2Star=StarNivel2;
        data.Nivel3Star=StarNivel3;
        data.Nivel4Star=StarNivel4;

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
        StarNivel1 = data.Nivel1Star;
        StarNivel2 = data.Nivel2Star;
        StarNivel3 = data.Nivel3Star;

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
        data.Bonus = false;

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
    public void PrintTimeInScreen()
    {
        timeText.text = "Tiempo: " + countdown;
    }
}
