using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public DifficultySettings easy   = new DifficultySettings();
    public DifficultySettings normal = new DifficultySettings();
    public DifficultySettings hard   = new DifficultySettings();
    
    public static DifficultySettings currentSettings;

    void Start()
    {
        InitializeDifficultySettings();
    }

    void InitializeDifficultySettings()
    {
        // Definindo os valores para o modo fácil
        easy.playerLives = 4;
        easy.playerAmmShoot = 5;
        easy.platformSpeed = 3.0f;
        easy.playerInveTime = 2.5f;
        easy.bossAttack1   = 20;

        // Definindo os valores para o modo normal
        normal.playerLives = 3;
        normal.playerAmmShoot = 3;
        normal.platformSpeed = 4.0f;
        normal.playerInveTime = 2.2f;
        normal.bossAttack1   = 25;

        // Definindo os valores para o modo difícil
        hard.playerLives = 2;
        hard.playerAmmShoot = 0;
        hard.platformSpeed = 5.0f;
        hard.playerInveTime = 2.0f;
        hard.bossAttack1   = 35;

    }

    public void SetDifficulty(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy":
                currentSettings = easy;
                break;
            case "Normal":
                currentSettings = normal;
                break;
            case "Hard":
                currentSettings = hard;
                break;
        }
    }
}