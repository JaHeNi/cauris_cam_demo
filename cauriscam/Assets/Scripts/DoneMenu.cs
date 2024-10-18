using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoneMenu : MonoBehaviour
{

    public void MainMenuButton()
    {
        // Play Now Button has been pressed, here you can initialize your game (For example Load a Scene called GameLevel etc.)
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

    public void QuitButton()
    {
        // Quit Game
        Application.Quit();
    }
}