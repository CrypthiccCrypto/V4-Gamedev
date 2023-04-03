using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject[] playerHexPrefab;
    [SerializeField] GameObject[] AIHexPrefab;
    [SerializeField] GameObject GridCornerPrefab;
    [SerializeField] GameObject GridBoundaryPrefab;
    [SerializeField] GameObject GridCentrePrefab;
    GameManager gameManager;
    [SerializeField] public static float inter_center_distance = 3.8f;
    private float y_factor = Mathf.Sqrt(3)/2;

    void Start() {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        CreateGrid(gameManager.GetBoardSize());
        
    }   

    void CreateGrid(int grid_size) {
        foreach(int idx in Board.allPossibleIndices)
        {
            int[] coords = gameManager.game.IndexToCubic(idx);
            bool b1 = ((int)Mathf.Abs(coords[0]) == grid_size - 1);
            bool b2 = ((int)Mathf.Abs(coords[1]) == grid_size - 1);
            bool b3 = ((int)Mathf.Abs(coords[2]) == grid_size - 1);

            int i1 = b1 ? 1 : 0;
            int i2 = b2 ? 1 : 0;
            int i3 = b3 ? 1 : 0;            

            if((i1 + i2 + i3) == 2) {   // corner hex piece
                Instantiate(GridCornerPrefab, Grid.GridToWorldCoordinates(coords[0], coords[1], coords[2]) - new Vector3(0, 1, 0), Quaternion.identity);
            }
            else if((i1 + i2 + i3) == 0) {  // inner piece
                Instantiate(GridCentrePrefab, Grid.GridToWorldCoordinates(coords[0], coords[1], coords[2]) - new Vector3(0, 1, 0), Quaternion.identity);
            }
            else {                          // edge hex piece
                if(coords[0] == grid_size - 1) {
                    float angleDegrees = -60;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                    Instantiate(GridBoundaryPrefab, Grid.GridToWorldCoordinates(coords[0], coords[1], coords[2]) - new Vector3(0, 1, 0), rot); // 60
                }
                else if(coords[0] == -grid_size + 1) {
                    float angleDegrees = 60;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                    Instantiate(GridBoundaryPrefab, Grid.GridToWorldCoordinates(coords[0], coords[1], coords[2]) - new Vector3(0, 1, 0), rot); //-60
                }
                else if(coords[1] == grid_size - 1) {
                    float angleDegrees = 180;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                    Instantiate(GridBoundaryPrefab, Grid.GridToWorldCoordinates(coords[0], coords[1], coords[2]) - new Vector3(0, 1, 0), rot); //0
                }
                else if(coords[1] == -grid_size + 1) {
                    Instantiate(GridBoundaryPrefab, Grid.GridToWorldCoordinates(coords[0], coords[1], coords[2]) - new Vector3(0, 1, 0), Quaternion.identity); //180
                }
                else if(coords[2] == grid_size - 1) {
                    float angleDegrees = -120;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                    Instantiate(GridBoundaryPrefab, Grid.GridToWorldCoordinates(coords[0], coords[1], coords[2]) - new Vector3(0, 1, 0), rot); //120
                }
                else if(coords[2] == -grid_size + 1) {
                    float angleDegrees = 120;
                    Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                    Instantiate(GridBoundaryPrefab, Grid.GridToWorldCoordinates(coords[0], coords[1], coords[2]) - new Vector3(0, 1, 0), rot); //-120
                }
            }
        }
    }

    public void PlaceTile(int x, int y, int z, int player) {

        int randomIndex = UnityEngine.Random.Range(0,4);

        if(player == (int)TURN.PLAYER_TURN) {
            Instantiate(playerHexPrefab[randomIndex], Grid.GridToWorldCoordinates(x, y, z), Quaternion.identity);
        }
        else {
            Instantiate(AIHexPrefab[randomIndex], Grid.GridToWorldCoordinates(x, y, z), Quaternion.identity);
        }
    }

    static public Vector3 GridToWorldCoordinates(int x, int y, int z) {
        return new Vector3(inter_center_distance * (y - x)/2, 0, (x + y) * Mathf.Sqrt(3)/2 * inter_center_distance);
    }

    static public Vector3 WorldToGridCoordinates(Vector3 worldCoordinates) {
        worldCoordinates.x /= inter_center_distance;
        worldCoordinates.z /= inter_center_distance;

        worldCoordinates.x = 2*worldCoordinates.x;
        worldCoordinates.x = Mathf.Round(worldCoordinates.x);
        worldCoordinates.x /= 2;

        worldCoordinates.z = 2*worldCoordinates.z/Mathf.Sqrt(3);
        worldCoordinates.z = Mathf.Round(worldCoordinates.z);
        worldCoordinates.z /= 2/Mathf.Sqrt(3);

        int i = Mathf.RoundToInt(worldCoordinates.z/Mathf.Sqrt(3) - worldCoordinates.x);
        int j = Mathf.RoundToInt(worldCoordinates.x + worldCoordinates.z/Mathf.Sqrt(3));
        int k = -(i + j);

        return new Vector3(i, j, k); 
    }
}
