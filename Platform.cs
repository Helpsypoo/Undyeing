using Godot;
using System;

public partial class Platform : AnimatableBody3D
{
	[Export] private bool _oscillate; 
	[Export] private float _oscillationPeriod;
	[Export] private float _initialPhaseOffset;
	private Vector3 _initialPosition;
	
	public override void _Ready()
	{
		// Freeze = true;
		// FreezeMode = FreezeModeEnum.Kinematic;
		
		_initialPosition = Position;
		if (_oscillate && _oscillationPeriod == 0)
		{
			GD.PushWarning("_oscillation period cannot be zero. Turning off oscillation.");
			_oscillate = false;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!_oscillate) return;
		Position = _initialPosition + Vector3.Up * Mathf.Sin(Mathf.Tau * (Time.GetTicksMsec() / 1000f / _oscillationPeriod + _initialPhaseOffset % 1));
	}
}
