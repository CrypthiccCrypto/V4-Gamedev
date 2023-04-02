using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private bool isPlaced = false;
    public void SetPlaced(bool placed) {
        isPlaced = placed;
    }

    public bool GetPlaced() {
        return isPlaced;
    }
}
