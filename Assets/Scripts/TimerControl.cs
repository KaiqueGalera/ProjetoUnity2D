using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerControl : MonoBehaviour
{
    public float timeLimit = 30f; // Tempo limite em segundos
    private float elapsedTime;

    private static TimerControl instance;

    void Awake()
    {
        // Garante que apenas uma instância do Timer exista
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre cenas
        }
        else
        {
            Destroy(gameObject); // Impede múltiplas instâncias
        }
    }

    void Start()
    {
        PlayerPrefs.DeleteAll();
        
        elapsedTime = PlayerPrefs.GetFloat("ElapsedTime", 0f);

        // Verifica se o tempo salvo já excede o limite
        if (elapsedTime >= timeLimit)
        {
            EndGame();
        }
    }

    void Update()
    {
        // Conta o tempo
        elapsedTime += Time.deltaTime;

        print(elapsedTime);

        if (elapsedTime >= timeLimit)
        {
            EndGame();
        }
    }

    
    void EndGame()
    {
        Debug.Log("Tempo esgotado. O jogo será encerrado.");
        SceneManager.LoadScene("GameOver");
    }

    void OnApplicationQuit()
{
    // Salva o tempo decorrido quando o jogo for fechado
    PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
    PlayerPrefs.Save();
}

}