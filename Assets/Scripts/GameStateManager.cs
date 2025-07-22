using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GameState
{
    Playing,
    DialogState
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; } // Instância estática para implementar o padrão Singleton.

    public GameState CurrentState { get; private set; } = GameState.Playing; // Propriedade pública para armazenar o estado atual do jogo.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Se não, define esta instância como a instância única... 
            DontDestroyOnLoad(gameObject); // ...e impede que seja destruída ao carregar uma nova cena.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState; // Atualiza o estado atual do jogo

        // Se o novo estado for DialogState, pausa o tempo do jogo (Time.timeScale = 0).
        // Caso contrário, define o tempo do jogo para o normal (Time.timeScale = 1)
        Time.timeScale = (newState == GameState.DialogState) ? 0 : 1;
    }
}