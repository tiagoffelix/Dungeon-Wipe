using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCoinsSO", menuName = "CoinsSO")]
public class CoinsSO : ScriptableObject
{
    [SerializeField] private int score;
    [SerializeField] private AudioSource sound;
    [SerializeField] private int timeToDespawn;

    public int Score
    {
        get { return score; }
    }

    public AudioSource Sound
    {
        get { return sound; }
    }
    public int TimeToDespawn
    {
        get { return timeToDespawn; }
    }
}
