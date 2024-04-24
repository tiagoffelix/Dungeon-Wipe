using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Potion", menuName = "Damage Potion")]
public class DamagePotion : Potion
{
    [SerializeField] private float damageBoost;

    public float DamageBoost
    {
        get { return damageBoost; }
        set { damageBoost = Mathf.Max(0, value); } // Ensure that damage boost cannot be negative
    }
}