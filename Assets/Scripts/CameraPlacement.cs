using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlacement : MonoBehaviour
{
    GameManager gameManager;

    public void Start() {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        int bs = gameManager.GetBoardSize();
        if(bs == 5)  {
            this.transform.position = new Vector3(0, 25, -42);
            this.transform.rotation = Quaternion.Euler(32, 0, 0);
        }
        else if(bs == 6) {
            this.transform.position = new Vector3(0, 30.5f, -44.9f);
            this.transform.rotation = Quaternion.Euler(35, 0, 0);
        }
        else if(bs == 7) {
            this.transform.position = new Vector3(0, 29.9f, -41.6f);
            this.transform.rotation = Quaternion.Euler(40, 0, 0);
        }
    }

}
