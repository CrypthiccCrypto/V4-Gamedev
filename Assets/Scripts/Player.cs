using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera cam;
    public LayerMask planeLayer;
    GameManager gameManager;
    [SerializeField] Grid grid;
    private GameObject prevTile;

    void Start() {
        grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    void Update() {
        // Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // if(Physics.Raycast(ray, out RaycastHit hitInfo, 100, planeLayer)) {
        //     // if(prevTile != null && prevTile != hitInfo.collider.gameObject) {
        //     //     if(!prevTile.GetComponent<Tile>().GetPlaced()) {
        //     //         prevTile.GetComponent<MeshRenderer>().enabled = false;
        //     //     }
        //     // }
        //     // hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
        //     // prevTile = hitInfo.collider.gameObject;

        //     // if(Input.GetMouseButtonDown(0)) {
        //     //     hitInfo.collider.gameObject.GetComponent<Tile>().SetPlaced(true);
        //     // }
        // }

        if(Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out RaycastHit hitInfo, 100, planeLayer)) {
                // if(prevTile != null && prevTile != hitInfo.collider.gameObject) {
                //     if(!prevTile.GetComponent<Tile>().GetPlaced()) {
                //         prevTile.GetComponent<MeshRenderer>().enabled = false;
                //     }
                // }
                // hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
                // prevTile = hitInfo.collider.gameObject;

                // if(Input.GetMouseButtonDown(0)) {
                //     hitInfo.collider.gameObject.GetComponent<Tile>().SetPlaced(true);
                // }
                
                Vector3 cubicCoordinates = Grid.WorldToGridCoordinates(hitInfo.point);
                if(gameManager.GetTurn() == (int)TURN.PLAYER_TURN) {
                    gameManager.UpdateGame((int)cubicCoordinates.x, (int)cubicCoordinates.y, (int)cubicCoordinates.z);
                }
            }
        }
    }
}
