using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMainMenu : MonoBehaviour
{
    public Slider slider;
    public Text text;
    bool aiFirst = true;
    public GameManager gameManager;

    void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        slider.value = gameManager.GetBoardSize();

        text.text = aiFirst ? "AI First" : "Player First";
        gameManager.SetAiFirst(aiFirst);
    }
    public void Swap() {
        aiFirst = !aiFirst;
        text.text = aiFirst ? "AI First" : "Player First";
        gameManager.SetAiFirst(aiFirst);
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
