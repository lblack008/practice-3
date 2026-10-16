using Godot;
using System;

public partial class Bullet : Area3D
{
	// параметры пули
	[Export]
	private float Speed = 100f;
	[Export]
	private float Damage = 10f;

	// сеттер для скорости
	public void SetSpeed(float speed)
	{
		Speed = speed;
	}

	// сеттер для урона
	public void SetDamage(float damage)
	{
		Damage = damage;
	}

	// движение пули
    public override void _PhysicsProcess(double delta)
    {
        Position += -Transform.Basis.Z * Speed * (float)delta;
    }

	// когда пуля появляется на сцене - она наносит урон тем сущностям, у которых есть здоровье
	public void OnBulletBodyEntered(Node3D body)
	{
		if (body is CharacterBody3D c)
		{
			HealthComponent h = c.GetNodeOrNull<HealthComponent>("HealthComponent");
			if (h != null)
			{
				h.TakeDamage(Damage);
			}
		}
		QueueFree();
	}
}
