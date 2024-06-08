using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleInteractable : MonoBehaviour
{
    void Start()
    {
        // Disable the renderer to make the GameObject invisible
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = false;
        }

        // Ensure the collider is enabled
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = true;
        }
    }
}
