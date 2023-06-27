using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPause : MonoBehaviour
{
    [SerializeField] private GameObject btnPausa;
    [SerializeField] private GameObject menuPausa;


   public void Pausa(){
         Time.timeScale = 0f;
         btnPausa.SetActive(false);
         menuPausa.SetActive(true);
   }

   public void Reanudar(){
         Time.timeScale = 1f;
         btnPausa.SetActive(true);
         menuPausa.SetActive(false);
   }
   public void Reinicio(){
        Time.timeScale = 1f;
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
   }

   public void Salir(){
            //Application.Quit();
            Time.timeScale = 1f;
            SceneManager.LoadScene("MenuPrincipal");
   }
}
