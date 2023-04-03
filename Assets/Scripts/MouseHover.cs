using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseHover : MonoBehaviour
{
    [SerializeField] MeshRenderer mesh;
    GameManager gameManager;
    bool gameManagerSet = false;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        gameManagerSet = true;
    }
    void OnMouseOver()
    {   
        if(gameManagerSet) {
            if(gameManager.GetTurn() != (int)TURN.NO_TURN) mesh.enabled = true;
        }
    }

    void OnMouseExit()
    {
        mesh.enabled = false;
    }

    private void OnMouseDown() {
        Destroy(gameObject);
    }
}
