using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameManager gameManager;
    void Start(){
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public void Continue() {
        gameManager.Continue();
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }

    public void Replay() {
        SceneManager.LoadScene(3);
    }
}
