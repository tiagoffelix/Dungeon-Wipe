using UnityEngine;

[CreateAssetMenu(fileName = "New Speed Potion", menuName = "Speed Potion")]
public class SpeedPotion : Potion
{
    [SerializeField] private float speedBoost;

    public float SpeedBoost
    {
        get { return speedBoost; }
        set { speedBoost = value; }
    }
}