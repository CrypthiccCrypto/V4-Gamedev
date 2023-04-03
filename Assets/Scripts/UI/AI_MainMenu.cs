using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AI_MainMenu : MonoBehaviour
{
    GameManager gameManager;
    void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public void Easy()
    {
        gameManager.SetDifficulty(DIFFICULTY.EASY);
    }
    public void Medium()
    {
        gameManager.SetDifficulty(DIFFICULTY.MEDIUM);
    }
    public void Hard()
    {
        gameManager.SetDifficulty(DIFFICULTY.HARD);
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
