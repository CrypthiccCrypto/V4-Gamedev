using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateGrid : MonoBehaviour
{
    [SerializeField] GameObject hexPrefab;
    [SerializeField] int grid_size;
    [SerializeField] public static float inter_center_distance = 3.8f;
    private float y_factor = Mathf.Sqrt(3)/2;

    void Start() {
        CreateGrid(grid_size);
    }   

    void CreateGrid(int grid_size) {
        for(int row = -grid_size + 1; row <= grid_size - 1; row++) {
            for(float i = -grid_size + 1 + Mathf.Abs(row)/2.0f; i <= grid_size - 1 - Mathf.Abs(row)/2.0f; i++) {
                Debug.Log(i + " " + row);
                Instantiate(hexPrefab, new Vector3(i * inter_center_distance, 0, y_factor* row * inter_center_distance), Quaternion.identity);
            }
        }
    }
}
