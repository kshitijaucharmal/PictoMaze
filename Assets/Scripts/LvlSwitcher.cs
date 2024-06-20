using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlSwitcher : MonoBehaviour
{
    public void StartGame(){
        Debug.Log("Start Game");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void MainMenu(){
        SceneManager.LoadScene(0);
    }
    public void QuitGame(){
        Application.Quit();
    }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
