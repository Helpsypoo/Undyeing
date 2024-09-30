using Godot;
using System;

public partial class Level_LMAO : Level
{
	private Vector3 _initialCollisionSize;
	private BoxShape3D _finalSectionCollisionShape3D => (BoxShape3D)GetNode<CollisionShape3D>("%FakeSection/CollisionShape3D").Shape;
	public override void _Ready()
	{
		base._Ready();
		
		_initialCollisionSize = _finalSectionCollisionShape3D.Size;
		_finalSectionCollisionShape3D.Size = Vector3.Zero;
	}

	public override void Death()
	{
		base.Death();

		_finalSectionCollisionShape3D.Size = _initialCollisionSize;
	}
}
