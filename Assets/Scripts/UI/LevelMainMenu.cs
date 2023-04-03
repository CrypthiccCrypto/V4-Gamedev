using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMainMenu : MonoBehaviour
{
    public Slider slider;
    public Text text;
    int turn = -1;
    public GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        slider.value = gameManager.GetBoardSize();

        text.text = turn == -1 ? "Player First" : "AI First";
        gameManager.SetTurn(turn);
    }
    public void Swap() {
        turn = -turn;
        text.text = (turn == -1) ? "Player First" : "AI First";
        Debug.Log(text.text);
        gameManager.SetTurn(turn);
    }

    public void Next() {
        gameManager.SetBoardSize((int)slider.value);
        SceneManager.LoadScene(3);
    }
    public void Back()
    {
        gameManager.SetBoardSize((int)slider.value);
        SceneManager.LoadScene(0);
    }
}
