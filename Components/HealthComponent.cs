using Godot;
using System;

[GlobalClass]
public partial class HealthComponent : Node
{
	// параметр здоровья 
	[Export]
	protected float MaxHealth = 100f;
	// родительскиий узел
	private Node3D parentNode;
	protected float CurrentHealth = 100f;

	// в начале игры происходит поиск родительского узла и задаётся здоровье
    public override void _Ready()
    {
        parentNode = GetParent<Node3D>();
		CurrentHealth = MaxHealth;
    }

	// получение урона
	public void TakeDamage(float damage)
	{
		CurrentHealth -= damage;
		if (CurrentHealth <= 0)
		{
			// если здоровье закончилось - смерть
			Die();
		}
	}

	// "смерть" объекта
	private void Die()
	{
		parentNode.QueueFree();
	}
}
