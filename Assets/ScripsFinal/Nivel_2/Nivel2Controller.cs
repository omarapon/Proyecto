using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class Nivel2Controller : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;
    public int score = 0;
    public int lives = 3;
    int cont = 0, cont2 = 0;
    private int StarNivel1 = 0;
    public int StarNivel2 = 0;
    private int StarNivel3 = 0;
    private int StarNivel4 = 0;
    void Start()
    {
        
    }

    void Update()
    {
        
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
        Estrellas();
        data.Score = score;
        data.Live = lives;
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
        StarNivel1 = data.Nivel1Star;
        StarNivel3 = data.Nivel3Star;
        StarNivel4 = data.Nivel4Star;

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

    public void Estrellas(){
        if(cont>=27) StarNivel2 += 1;
        if(cont2>=2) StarNivel2 += 1;
    }
    public void Matar(){
        cont2 += 1;
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
    public void GanarPuntos(int puntos)
    {
        score += puntos;
        cont += 1;
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

