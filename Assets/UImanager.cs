using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{
   public void StartGame()
   {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
   }
   public void MainMenu()
   {
       SceneManager.LoadScene("MainMenu");
       Time.timeScale = 1;
   }
   public void QuitGame()
   {
       Application.Quit();
   }
}
