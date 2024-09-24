using Godot;
using System;

public partial class PathRepeater : PathFollow3D
{
	[Export] private float _period = 10;
	public override void _PhysicsProcess(double delta)
	{
		if (_period == 0) return;
		ProgressRatio += (float)delta / _period;
	}
}
