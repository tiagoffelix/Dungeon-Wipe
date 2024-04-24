using UnityEngine;

[CreateAssetMenu(fileName = "New Health Potion", menuName = "Health Potion")]
public class HealthPotion : Potion
{
    [SerializeField] private int health;

    public int Health
    {
        get { return health; }
        set { health = Mathf.Max(0, value); } // Ensure that health cannot be negative
    }
}