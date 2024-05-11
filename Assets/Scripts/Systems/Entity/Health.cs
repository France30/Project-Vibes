using UnityEngine;
using UnityEngine.UI;

public class Health
{ 
	private float _currentHealth;
	private Image _healthBar;

	public float MaxHealth { get; set; }
	public float CurrentHealth
	{
		get => _currentHealth;
		set
		{
			_currentHealth = value;
			_currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);
			OnHealthUpdated?.Invoke(_currentHealth, MaxHealth);

			if (_healthBar != null) UpdateHealthBar();
		}
	}

	public delegate void HealthUpdate(float health, float maxHealth);
	public event HealthUpdate OnHealthUpdated;


	public Health(float maxHealth, Image healthBar = null)
	{
		this._healthBar = healthBar;
		MaxHealth = maxHealth;
		CurrentHealth = MaxHealth;
	}

	private void UpdateHealthBar()
	{
		_healthBar.fillAmount = _currentHealth / MaxHealth;
	}
}
