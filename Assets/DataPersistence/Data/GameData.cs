using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData 
{
    public int          heroLifes; // O número atual de vidas para ser carregado após inciar um jogo já existente
    public int          heroHealth; // O número atual de vidas para ser carregado após inciar um jogo já existente
    public int          deaths;
    public int          shootAmm;
    public int          timeLimit;
    public int          bossDmg;
    // public Vector3      heroPosition;

    // Os valores definidos no construtor serão os valores default. O jogo começa com eles caso não possua dados para carregar
    public GameData()
    {
        this.heroLifes  = 4;
        this.heroHealth = 100;
        this.deaths     = 0;
        this.shootAmm   = 5;
        this.timeLimit  = 3000;
        this.bossDmg    = 30;

        // this.heroPosition = new Vector3(-4.29f, -2.015f, 0);
    }
}