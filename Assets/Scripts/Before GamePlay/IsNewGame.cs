using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsNewGame : MonoBehaviour
{
    public bool     isNewGame = false;
    private static  IsNewGame Instance;

    void Awake()
    {
     
        if (Instance != null)
        {
            // Se outra instância já existe, destrua este objeto para evitar duplicatas
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    void  Start ()
    {
        
        DontDestroyOnLoad(gameObject);
    }
    
    public void     isNew()
    {
        isNewGame = true;
    }
    public void     isntNew()
    {
        isNewGame = false;
    }
}
