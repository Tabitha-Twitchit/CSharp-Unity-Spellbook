using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Material_Multi : MonoBehaviour
{
    public Material mat1;
    public Material mat2;
    public Material mat3; 
    public Material mat4;
    public Material mat5;
    public Material mat6;
    private Material originalMat;
    private Renderer rend;
    private int matstate;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        originalMat = GetComponent<Renderer>().material;
        matstate = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("activator"))
            matstate = matstate + 1;
            MatChange();
    }

    void MatChange()
    {
        if (matstate == 1)
        {
            rend.material = mat1;
        }

        if (matstate == 2)
        {
            rend.material = mat2;
        }
        
        if (matstate == 3)
        {
            rend.material = mat3;
        }

        if (matstate == 4)
        {
            rend.material = mat4;
        }

        if (matstate == 5)
        {
            rend.material = mat5;
        }

        if (matstate == 6)
        {
            rend.naterial = mat6;
        }

        if (matstate == 7)
        {
            rend.material = originalMat;
            matstate = 0;
        }

    }

}
