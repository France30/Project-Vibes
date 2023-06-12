using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _maxHealth;

    private Health _health;


    private void Awake()
    {
        _health = new Health(_maxHealth);
    }
}
