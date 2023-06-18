using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private Health _health;


    private void Awake()
    {
        _health = new Health(_maxHealth);
    }
}
