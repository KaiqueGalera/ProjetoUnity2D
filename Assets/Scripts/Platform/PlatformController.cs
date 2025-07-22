using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformController : MonoBehaviour
{   
    public  Transform[]         positions;
    public  Transform           platform;
    public  float               speedPlatform;
    private int                 idTarget;

    // Start is called before the first frame update
    void Start()
    {
        platform.position = positions[0].position; // Garante que a posição da plataforma seja igual a da transformada no ponto 0

        idTarget = 1;
    }

    // Update is called once per frame
    void Update()
    {
        platform.position = Vector3.MoveTowards(platform.position, positions[idTarget].position, speedPlatform * Time.deltaTime); // Desloca a plataforma dado a posição de origem, posição alvo e a velocidade de deslocamento

        if (platform.position == positions[idTarget].position) // Caso a posição da plataforma tenha atingido a posição destino seguinte
        {
            idTarget++; // Muda a id do alvo para o seguinte

            if (idTarget == positions.Length) // Caso tenha atingido a posição final
            {
                idTarget = 0; // Retorna ao ponto de origem
            }
        }

    }
}
