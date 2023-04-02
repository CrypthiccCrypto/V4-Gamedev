using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseHover : MonoBehaviour
{
    [SerializeField] GameObject translucentHexPrefab;

    void OnMouseOver()
    {
        Instantiate(translucentHexPrefab, transform.position, Quaternion.identity);
    }
}
