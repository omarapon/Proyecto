using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPixelArt : MonoBehaviour
{
    public void OpenScene(int index)
    {
        SceneManager.LoadScene(index);
    }
   

   public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
    public void menuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }   
}
