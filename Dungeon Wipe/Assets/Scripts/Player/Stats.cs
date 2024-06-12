using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "Stats")]
/// <summary>
/// Represents the stats and configuration settings for the player.
/// </summary>
public class Stats : ScriptableObject
{
    [SerializeField] private float health; // The player's health
    [SerializeField] private float damage; // The player's damage dealt
    [SerializeField] private float arrowDamage; // The damage dealt by the player's arrows
    [SerializeField] private float attackCooldown; // The cooldown between player attacks
    [SerializeField] private bool dead; // Indicates whether the player is dead
    [SerializeField] private int sensitivity; // The player's mouse sensitivity
    [SerializeField] private float volume; // The volume for the game

    [SerializeField] private int score; // The player's score
    [SerializeField] private int numberOfSpawns; // Number of times the player has spawned

    /// <summary>
    /// Gets or sets the player's health.
    /// </summary>
    public float Health
    {
        get { return this.health; }
        set { this.health = value; }
    }

    /// <summary>
    /// Gets or sets the player's damage dealt.
    /// </summary>
    public float Damage
    {
        get { return this.damage; }
        set { this.damage = value; }
    }

    /// <summary>
    /// Gets or sets the damage dealt by the player's arrows.
    /// </summary>
    public float ArrowDamage
    {
        get { return this.arrowDamage; }
        set { this.arrowDamage = value; }
    }

    /// <summary>
    /// Gets or sets the cooldown between player attacks.
    /// </summary>
    public float AttackCooldown
    {
        get { return this.attackCooldown; }
        set { this.attackCooldown = value; }
    }

    /// <summary>
    /// Gets or sets the volume.
    /// </summary>
    public float Volume
    {
        get { return this.volume; }
        set { this.volume = value; }
    }

    /// <summary>
    /// Gets or sets the player's mouse sensitivity.
    /// </summary>
    public int Sensitivity
    {
        get { return this.sensitivity; }
        set { this.sensitivity = value; }
    }

    /// <summary>
    /// Gets or sets the player's score.
    /// </summary>
    public int Score
    {
        get { return this.score; }
        set { this.score = value; }
    }

    /// <summary>
    /// Gets or sets the number of times the player has spawned.
    /// </summary>
    public int NumberOfSpawns
    {
        get { return this.numberOfSpawns; }
        set { this.numberOfSpawns = value; }
    }

    /// <summary>
    /// Gets or sets whether the player is dead.
    /// </summary>
    public bool Dead
    {
        get { return this.dead; }
        set { this.dead = value; }
    }

    /// <summary>
    /// Adds health to the player, up to a maximum of 100.
    /// </summary>
    /// <param name="healthReceived">Amount of health to add.</param>
    public void AddHealth(int healthReceived)
    {
        Health += healthReceived;

        if (Health > 100)
        {
            Health = 100;
        }
    }

    /// <summary>
    /// Reduces the player's health by the specified damage amount.
    /// </summary>
    /// <param name="damage">Amount of damage to take.</param>
    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Health = 0;
        }
    }
}
