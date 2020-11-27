using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script attached to the prefabs of line renderer to implement Color differentiation
public class Grayscale : MonoBehaviour
{
    public Material grayMat;        // Gray material
    public Material defaultMat;     // Default material can be brown or green depending on prefab
    private LineRenderer line;


    public static bool grayscale = false;   // Bool to match the toggle value from the UI
    void Start()
    {
        line = GetComponent<LineRenderer>();
        
    }

    private void Update()
    {
           if (grayscale == true)
            {
                line.material = grayMat;    // If in grayscale mode, assign the gray material to the line
            }
            else
            {
                line.material = defaultMat;  // If in Color mode, assign the default material to the line
        }
       
    }
    
}
