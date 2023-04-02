using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] GameObject playerHexPrefab;
    [SerializeField] GameObject AIHexPrefab;
    [SerializeField] int grid_size;
    [SerializeField] public static float inter_center_distance = 3.8f;
    private float y_factor = Mathf.Sqrt(3)/2;

    void Start() {
        // CreateGrid(grid_size);
    }   

    // void CreateGrid(int grid_size) {
    //     for(int row = -grid_size + 1; row <= grid_size - 1; row++) {
    //         for(float i = -grid_size + 1 + Mathf.Abs(row)/2.0f; i <= grid_size - 1 - Mathf.Abs(row)/2.0f; i++) {
    //             Debug.Log(i + " " + row);
    //             Instantiate(hexPrefab, new Vector3(i * inter_center_distance, 0, y_factor* row * inter_center_distance), Quaternion.identity);
    //         }
    //     }
    // }

    public void PlaceTile(int x, int y, int z, int player) {
        if(player == (int)TURN.PLAYER_TURN) {
            Instantiate(playerHexPrefab, Grid.GridToWorldCoordinates(x, y, z), Quaternion.identity);
        }
        else {
            Instantiate(AIHexPrefab, Grid.GridToWorldCoordinates(x, y, z), Quaternion.identity);
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
