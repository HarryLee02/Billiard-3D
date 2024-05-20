using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void EnterGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void ReturnMainMenu() {
        SceneManager.LoadScene(0);
    }
    public void EnterTraining() {
        SceneManager.LoadScene(2);
    }
    public void QuitGame() {
        Debug.Log("Quit");
        Application.Quit();
    }
}
