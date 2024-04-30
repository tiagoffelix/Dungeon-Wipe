using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Stats")]
public class Stats : ScriptableObject
{
    [SerializeField] private float health;

    [SerializeField] private float damage;

    [SerializeField] private float arrowDamage;

    [SerializeField] private float attackCooldown;

    [SerializeField] private bool dead;

    [SerializeField] private int sensitivity;

    [SerializeField] private AudioClip walkingSound;
    [SerializeField] private AudioClip swordSound;
    [SerializeField] private AudioClip slashSound;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip bowSound;
    [SerializeField] private AudioClip spikesSound;
    [SerializeField] private AudioClip parrySound;
    [SerializeField] private AudioClip shieldedSound;
    [SerializeField] private AudioClip shieldBreakSound;
    [SerializeField] private AudioClip coinsSound;
    [SerializeField] private AudioClip drawSwordSound; 

    [SerializeField] private int score;

    [SerializeField] private int numberOfSpawns;

    [SerializeField] private int levelMinutes;

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

    public float AttackCooldown
    {
        get { return this.attackCooldown; }
        set { this.attackCooldown = value; }
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

    public AudioClip WalkingSound
    {
        get { return walkingSound; }
    }

    public AudioClip SwordSound
    {
        get { return swordSound; }
    }

    public AudioClip DrawSwordSound
    {
        get { return drawSwordSound; }
    }
    public AudioClip SlashSound
    {
        get { return slashSound; }
    }

    public AudioClip HitSound
    {
        get { return hitSound; }
    }
    public AudioClip BowSound
    {
        get { return bowSound; }
    }
    public AudioClip SpikesSound
    {
        get { return spikesSound; }
    }
    public AudioClip ParrySound
    {
        get { return parrySound; }
    }
    public AudioClip ShieldedSound
    {
        get { return shieldedSound; }
    }
    public AudioClip ShieldBreakSound
    {
        get { return shieldBreakSound; }
    }
    public AudioClip CoinsSound
    {
        get { return coinsSound; }
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
        }
    }
}
