using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditorInternal.Profiling.Memory.Experimental.FileFormat;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyType", menuName = "Enemy Type")]
public class EnemyType : ScriptableObject
{
    [SerializeField] private string typeName;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float attackRange;
    [SerializeField] private int baseDamage;
    [SerializeField] private int health;
    [SerializeField] private int points;
    [SerializeField] private AudioSource attackSound;
    [SerializeField] private AudioSource deathSound;

    // Encapsulated properties
    public string TypeName
    {
        get { return typeName; }
    }

    public float AttackCooldown
    {
        get { return attackCooldown; }
    }

    public float AttackRange
    {
        get { return attackRange; }
    }

    public int BaseDamage
    {
        get { return baseDamage; }
    }

    public int Health
    {
        get { return health; }
    }

    public int Points
    {
        get { return points; }
    }

    public AudioSource AttackSound
    {
        get { return attackSound; }
    }
    public AudioSource DeathSound
    {
        get { return deathSound; }
    }
}
