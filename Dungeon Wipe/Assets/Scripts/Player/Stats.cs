using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Stats")]
public class Stats : ScriptableObject
{
    [SerializeField] private float health;

    [SerializeField] private float damage;

    [SerializeField] private float arrowDamage;

    [SerializeField] private bool dead;

    [SerializeField] private int sensitivity;

    [SerializeField] private AudioSource walkingSound;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource hitSound;
    [SerializeField] private AudioSource bowSound;

    [SerializeField] private int score;

    [SerializeField] private int numberOfSpawns;

    [SerializeField] private int levelMinutes;

    [SerializeField] private string selectedLevelPath;

    public string SelectedLevelPath
    {
        get { return selectedLevelPath; }
        set { selectedLevelPath = value; }
    }

    public float Health 
    {
        get { return this.health; }
        set {  this.health = value; }
    }

    public float Damage
    {
        get { return this.damage; }
        set { this.damage = value; }
    }

    public float ArrowDamage
    {
        get { return this.arrowDamage; }
        set { this.arrowDamage = value; }
    }

    public int Sensitivity
    {
        get { return this.sensitivity; }
        set { this.sensitivity = value; }
    }

    public int Score
    {
        get { return this.score; }
        set { this.score = value; }
    }

    public int NumberOfSpawns
    {
        get { return this.numberOfSpawns; }
        set { this.numberOfSpawns = value; }
    }

    public int LevelMinutes
    {
        get { return this.levelMinutes; }
        set { this.levelMinutes = value; }
    }

    public bool Dead
    {
        get { return this.dead; }
        set { this.dead = value; }
    }

    public AudioSource WalkingSound
    {
        get { return walkingSound; }
    }

    public AudioSource AttackSound
    {
        get { return attackSound; }
    }

    public AudioSource HitSound
    {
        get { return hitSound; }
    }
    public AudioSource BowSound
    {
        get { return bowSound; }
    }


    public void AddHealth(int healthReceived)
    {
        Health += healthReceived;

        if (Health > 100)
        {
            Health = 100;
        }
    }

    public void TakeDamage(float damage) {

        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
            Dead = true;
        }
    }
}
