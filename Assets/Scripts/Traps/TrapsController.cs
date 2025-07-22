using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapsController : MonoBehaviour
{
    private     GameController  _GameController;

    [Header("Trap Animation")]
    private     Animator        trapMouthAn;
    private     bool            isMouthClosed;

    [Header("Colision")]
    private     BoxCollider2D   trapCo2D;
    private     int             n;

    // Start is called before the first frame update
    void Start()
    {
        _GameController = FindAnyObjectByType(typeof(GameController)) as GameController;

        trapMouthAn = GetComponent<Animator>();
        trapCo2D    = GetComponent<BoxCollider2D>();

        Randomizer(1,2);
    }


    void FixedUpdate()
    {
        if (isMouthClosed == false)
        {
            StartCoroutine("MouthTrap");
        }
        
    }

    void  EnableCo2D() // Função para habilitar o colisor da trap *É evocada na própria unity pelo animator
    {
        trapCo2D.enabled = true;
    }
    
    void DisableCo2D() // Função para desabilitar o colisor da trap *É evocada na própria unity pelo animator
    {
        trapCo2D.enabled = false;
    }

    int Randomizer(int min, int max)
    {   
        return n = Random.Range(min, max + 1);
    }
    public IEnumerator MouthTrap() // Coroutine para tocar animação da trap e consequentemente a sua mecânica
    {   
        isMouthClosed = true;
        yield return new WaitForSeconds(_GameController.mouthTimeTrap); // Espera o tempo definido no GameController para a trap acionar    
        if (n == 1)
        {
            trapMouthAn.SetTrigger("TrMouthClosed"); // Ativa o trigger para rodar a animação 
        } else if (n == 2)
        {
            trapMouthAn.SetTrigger("TrMouthClosed2"); // Ativa o trigger 2 para rodar a animação 
        }


        isMouthClosed = false;
    }


}
