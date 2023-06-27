using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ScripsNiveles : MonoBehaviour
{
    [SerializeField] private GameObject menuCreditos;
    [SerializeField] private GameObject menuNiveles;
    [SerializeField] private GameObject menuPrincipal;
    [SerializeField] private GameObject btnRunnerExplorer;
    [SerializeField] private GameObject btnNivel;
    [SerializeField] private GameObject btnJugarO;
    [SerializeField] private GameObject btnOpcionesO;
    [SerializeField] private GameObject btnCreditosO;
    [SerializeField] private GameObject btnSalirO;
    public int StarNivel1 = 0;
    public int StarNivel2 = 0;
    public int StarNivel3 = 0;
    public int StarNivel4 = 0;
    public Button Ni1;
    public Button Ni2;
    public Button Ni3;
    public Button Ni4;
    public Sprite Estrella0;
    public Sprite Estrella1;
    public Sprite Estrella2;
    public Sprite Estrella3;

      void Start()
      {
        
            
         LoadGame();   

      }
    public void AbrirMenu(){
         Time.timeScale = 1f;
         menuPrincipal.SetActive(true);
         btnRunnerExplorer.SetActive(false);
         
   }

   public void LoadGame()
    {
      var filePath = Application.persistentDataPath + "/guardar.dat";
      FileStream file;

      if (File.Exists(filePath))
      file = File.OpenRead(filePath);
      else
      {
      Debug.LogError("No se encontro archivo");
      return;
      }

      BinaryFormatter bf = new BinaryFormatter();
      GameData data = (GameData)bf.Deserialize(file);
      file.Close();

      //usar datos generados
      //StarNivel1 = 1;
      //StarNivel2 = 0;
      //StarNivel3 = 3;
      //StarNivel4 = 1;

      //usar datos guardados
      StarNivel1 = data.Nivel1Star;
      StarNivel2 = data.Nivel2Star;
      StarNivel3 = data.Nivel3Star;
      StarNivel4 = data.Nivel4Star;



        Debug.Log(StarNivel1 + "-" + StarNivel2 + "-" + StarNivel3 + "-" + StarNivel4);
      imprimirStar();
      

    }

    public void imprimirStar(){
      // Obtener el componente Image del botón
      Image image1 = Ni1.GetComponent<Image>();
      // Asignar el nuevo sprite al componente Image
      switch(StarNivel1){
            case 0: image1.sprite = Estrella0;
                break;
            case 1: image1.sprite = Estrella1;
                break;
            case 2: image1.sprite = Estrella2;
                break;
            case 3: image1.sprite = Estrella3;
                break;
            default:
                Debug.Log("Opción no válida");
                break;
      }
      Image image2 = Ni2.GetComponent<Image>();
      // Asignar el nuevo sprite al componente Image
      switch(StarNivel2){
            case 0: image2.sprite = Estrella0;
                break;
            case 1: image2.sprite = Estrella1;
                break;
            case 2: image2.sprite = Estrella2;
                break;
            case 3: image2.sprite = Estrella3;
                break;
      }
      
      Image image3 = Ni3.GetComponent<Image>();
      // Asignar el nuevo sprite al componente Image
      switch(StarNivel3){
            case 0: image3.sprite = Estrella0;
                break;
            case 1: image3.sprite = Estrella1;
                break;
            case 2: image3.sprite = Estrella2;
                break;
            case 3: image3.sprite = Estrella3;
                break;
      }
      
      Image image4 = Ni4.GetComponent<Image>();
      // Asignar el nuevo sprite al componente Image
      switch(StarNivel4){
            case 0: image4.sprite = Estrella0;
                break;
            case 1: image4.sprite = Estrella1;
                break;
            case 2: image4.sprite = Estrella2;
                break;
            case 3: image4.sprite = Estrella3;
                break;
      }
      
    }

    public void AbrirNiveles(){
         Time.timeScale = 0f;
         btnNivel.SetActive(false);
         menuNiveles.SetActive(true);
         btnJugarO.SetActive(false);
         btnOpcionesO.SetActive(false);
         btnCreditosO.SetActive(false);
         btnSalirO.SetActive(false);
   }

   public void CerrarNiveles(){
         Time.timeScale = 1f;
         menuNiveles.SetActive(false);
         Aparacer();

   }
   public void AbrirCreditos(){
         Time.timeScale = 0f;
         btnNivel.SetActive(false);
         menuCreditos.SetActive(true);
         btnJugarO.SetActive(false);
         btnOpcionesO.SetActive(false);
         btnCreditosO.SetActive(false);
         btnSalirO.SetActive(false);
   }
   public void CerrarCreditos(){
         Time.timeScale = 1f;
         menuCreditos.SetActive(false);
         Aparacer();
         }

   public void Aparacer(){
        btnNivel.SetActive(true);
        btnJugarO.SetActive(true);
        btnOpcionesO.SetActive(true);
        btnCreditosO.SetActive(true);
        btnSalirO.SetActive(true);
   }

    public void Nivel1(){
            Time.timeScale = 1f;
            SceneManager.LoadScene("Nivel_8");
    }
    public void Nivel2(){
        Time.timeScale = 1f;
          SceneManager.LoadScene("Nivel_2");
    }
    public void Nivel3(){
        Time.timeScale = 1f;
          SceneManager.LoadScene("Nivel_3");
    }
    public void Nivel4(){
        Time.timeScale = 1f;
          SceneManager.LoadScene("Nivel_10");
    }
}

