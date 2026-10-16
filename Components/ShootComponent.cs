using Godot;
using System;

[GlobalClass]
public partial class ShootComponent : Node3D
{
	// параметры пули
	[Export]
	private PackedScene BulletScene;
	[Export]
	private float BulletDamage = 10f;
	[Export]
	private float BulletSpeed = 100f;
	[Export]
	private float FireRate = 2f;

	private Node3D Target;
	// таймер для количества выстрелов в секунду
	private Timer shootTimer = new Timer();

	// в начале игры задаётся время перезарядки выстрелов
    public override void _Ready()
    {
        shootTimer.WaitTime = 1.0f / FireRate;
		shootTimer.OneShot = false;
		AddChild(shootTimer);

		shootTimer.Timeout += () => TryShoot();
    }

	// попытка выстрела
	private void TryShoot()
	{
		if (BulletScene != null)
		{
			SpawnBullet();
		}
		else
		{
			GD.Print("No bullet scene attached!");
		}
	}

	// создание пули на сцене при выстреле
	private void SpawnBullet()
	{
		Bullet bullet = BulletScene.Instantiate<Bullet>();
		GetTree().CurrentScene.AddChild(bullet);
		bullet.GlobalPosition = GlobalPosition;
		bullet.GlobalRotation = GlobalRotation;
		bullet.SetDamage(BulletDamage);
		bullet.SetSpeed(BulletSpeed);
	}

	// включение пушки
	private void TurnOnCannon()
	{
		if (shootTimer.IsStopped()) shootTimer.Start();
	}

	// выключение пушки
	private void TurnOffCannon()
	{
		if (!shootTimer.IsStopped()) shootTimer.Stop();
	}

	// сеттер для цели
	public void SetTarget(Node3D target)
	{
		Target = target;
	}

	// если цель есть - выстрел, иначе - пушка просто крутится 
    public override void _Process(double delta)
    {
		// проверка, существует ли цель
        if (GodotObject.IsInstanceValid(Target))
		{
			GD.Print("I caught an enemy!");
			LookAt(Target.GlobalPosition);
			TurnOnCannon();
		}
		else
		{
			// если после удаления цели она всё ещё не нулевая - она зануляется искусственно
			if (Target != null) 
			{
				Target = null;
			}

			// выключение пушки
			TurnOffCannon();
			Rotation = new Vector3(
				Rotation.X,
				Rotation.Y + (float)delta * 0.5f,
				Rotation.Z
			);
		}
    }

}
