using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MouseHover : MonoBehaviour
{
    [SerializeField] MeshRenderer mesh;
    void OnMouseOver()
    {
        mesh.enabled = true;
    }

    void OnMouseExit()
        {
            mesh.enabled = false;
        }

    private void OnMouseDown() {
        Destroy(gameObject);
    }
}
