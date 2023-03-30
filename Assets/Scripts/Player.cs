using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera cam;
    public LayerMask tileLayer;
    [SerializeField] GenerateGrid generateGrid;
    private GameObject prevTile;

    void Start() {
        generateGrid = GameObject.FindGameObjectWithTag("Grid Generator").GetComponent<GenerateGrid>();
    }
    void Update() {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hitInfo, 100, tileLayer)) {
            if(prevTile != null && prevTile != hitInfo.collider.gameObject) {
                if(!prevTile.GetComponent<Tile>().GetPlaced()) {
                    prevTile.GetComponent<MeshRenderer>().enabled = false;
                }
            }
            hitInfo.collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
            prevTile = hitInfo.collider.gameObject;

            if(Input.GetMouseButtonDown(0)) {
                hitInfo.collider.gameObject.GetComponent<Tile>().SetPlaced(true);
            }
        }
    }
}
