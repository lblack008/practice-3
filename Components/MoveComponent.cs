using Godot;
using System;

[GlobalClass]
public partial class MoveComponent : Node
{
	// параметры для движения
	[Export]
	protected float Speed = 50f;
	[Export]
	protected float StopRadius = 0.5f;
	protected Node3D Target;
	protected CharacterBody3D parentNode;

	// в начале игры проверка, что родительский узел - character3d
    public override void _Ready()
    {
        if (GetParent() is CharacterBody3D p)
		{
			parentNode = p;
		}
    }

	// сеттер для цели
	public void SetTarget(Node3D target)
	{
		Target = target;
	}

	// если родительский узел существует - он двигается
    public override void _PhysicsProcess(double delta)
    {
        if (parentNode != null)
		{
			Move(parentNode);
		}
    }

	// движение
	private void Move(CharacterBody3D p)
	{
		// задаётся позиция цели (если её нет, движение в (0, 0, 0))
		Vector3 targetDistance;
		Vector3 position = Vector3.Zero;
		if (Target != null)
		{
			position = Target.GlobalPosition;
		}
		targetDistance = position - p.GlobalPosition;
		Vector3 direction = targetDistance.Normalized();
		
		Vector3 velocity = Vector3.Zero;
		if (targetDistance.Length() > StopRadius)
		{
			velocity = new Vector3(direction.X * Speed, 0, direction.Z * Speed);
		}
		// движение к цели
		p.Velocity = velocity;
		p.MoveAndSlide();
		GD.Print("My target is " + position);
	}
}
