using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseHover : MonoBehaviour
{
    [SerializeField] MeshRenderer mesh;
    GameManager gameManager;

    void Start()
    {
                gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }
    void OnMouseOver()
    {
        if(gameManager.GetTurn() != (int)TURN.NO_TURN) mesh.enabled = true;
    }

    void OnMouseExit()
        {
            mesh.enabled = false;
        }

    private void OnMouseDown() {
        Destroy(gameObject);
    }
}
