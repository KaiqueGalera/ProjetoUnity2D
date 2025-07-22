using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOffSet : MonoBehaviour
{   
    private Renderer        meshRenderer;
    private Material        currentMaterial;
    public  float           incrementOffSet;
    public  float           speedSet;
    public  string          sortingLayer;
    public  int             orderInLayer;
    private float           offSet;        
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer =                  GetComponent<Renderer>(); // Acessa o componente
        meshRenderer.sortingLayerName = sortingLayer; // Define o noma da camada do meshRenderer
        meshRenderer.sortingOrder     = orderInLayer; // Define a posição da camada
        
        currentMaterial =               meshRenderer.material; // Define o material atual como sendo o definido para o meshRenderer no inspector da unity
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        offSet += incrementOffSet;

        currentMaterial.SetTextureOffset("_MainTex", new Vector2(offSet * speedSet, 0));
    }
}
