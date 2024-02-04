using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlawColor : MonoBehaviour
{
    public Material highlightedMaterial;
    public bool isTagged = false;
    private Material normalMaterial;


   
   
    void Start()
    {
        normalMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTagged)
        {
            HighlightMaterial(highlightedMaterial);
        }
        else
            HighlightMaterial(normalMaterial);

    }
    private void HighlightMaterial(Material material)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = material;
        }
    }
}
