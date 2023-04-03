using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
    
    public void Replay() {
        SceneManager.LoadScene(3);
    }

}
