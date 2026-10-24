using Godot;
using System;

[GlobalClass]
public partial class VisibilityComponent : Area3D
{
	// другие компоненты
	[Export]
	public MoveComponent moveComponent;
	[Export]
	public ShootComponent shootComponent;
	private Node3D ParentNode;
	private Godot.Collections.Array<Node3D> detectedBodies = new Godot.Collections.Array<Node3D>();
	private Node3D Target;

	// проверка работы коллизии 
	public override void _Ready()
	{
		ParentNode = GetParent<Node3D>();

		if (this.GetNodeOrNull<CollisionShape3D>("CollisionShape3D") != null)
		{
			this.BodyEntered += (Node3D body) => OnBodyEntered(body);
			this.BodyExited += (Node3D body) => OnBodyExited(body);

			// проверка объектов, которые уже находятся внутри зоны коллизии
			Godot.Collections.Array<Node3D> overlappingBodies = GetOverlappingBodies();
			for (int i = 0; i < overlappingBodies.Count; i++)
			{
				OnBodyEntered(overlappingBodies[i]);
			}
		}
	}

	// проверка, что объекты принадлежат к одной группе объектов
	private void OnBodyEntered(Node3D body)
	{
		bool isInSameGroup = false;
		if (
			(body.IsInGroup("Enemy") && ParentNode.IsInGroup("Enemy")) 
			||
			(body.IsInGroup("Player") && ParentNode.IsInGroup("Player")))
		{
			isInSameGroup = true;
		}
		// если объекты в разных группах - они добавляются в список объектов для атаки
		if (!isInSameGroup)
		{
			HealthComponent h = body.GetNodeOrNull<HealthComponent>("HealthComponent");
			if (h != null)
			{
				detectedBodies.Add(body);
			}
		}
	}

	// удаление объекта из списка для атаки, если он вышел из зоны коллизии
	private void OnBodyExited(Node3D body)
	{
		detectedBodies.Remove(body);
	}

	// поиск ближайшего объекта для атаки
	public override void _Process(double delta)
	{
		Node3D closestBody = null;
		for (int i = 0; i < detectedBodies.Count; i++)
		{
			Vector3 distToTarget = GlobalPosition - detectedBodies[i].GlobalPosition;
			if ((closestBody == null) || distToTarget.Length() < 
			    (GlobalPosition - closestBody.GlobalPosition).Length())
			{
				closestBody = detectedBodies[i];
			}
		}
		Target = closestBody;
		if (shootComponent != null)
		{
			shootComponent.SetTarget(Target);
		}
		if (moveComponent != null)
		{
			moveComponent.SetTarget(Target);
		}
	}
}
