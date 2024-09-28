using Godot;
using System;

public partial class Platform : AnimatableBody3D
{
	private Transform3D _initialTransform;
	
	[Export] private Vector3 _wiggleAmplitude;
	[Export] private Vector3 _wigglePeriod;
	[Export] private Vector3 _wigglePhaseOffset;
	
	[Export] private float _rotatePeriod;
	[Export] private float _rotatePhaseOffset;
	[Export] private Vector3 _rotateAxis;

	[Export] private PathFollow3D _pathFollower;
	
	public override void _Ready()
	{
		_initialTransform = GetTransform();
	}

	public override void _PhysicsProcess(double delta)
	{

		if (Engine.IsEditorHint()) return;
		var seconds = Time.GetTicksMsec() / 1000f;

		var newTransform = _initialTransform;
		if (_rotatePeriod != 0)
			newTransform = newTransform.RotatedLocal(_rotateAxis.Normalized(), seconds * Mathf.Tau / _rotatePeriod);
		
		if (_wigglePeriod.LengthSquared() != 0)
		{
			var wiggleVector = new Vector3(
				_wigglePeriod.X == 0 ? 0 : _wiggleAmplitude.X * Mathf.Sin(Mathf.Tau * (seconds / _wigglePeriod.X + _wigglePhaseOffset.X % 1)),
				_wigglePeriod.Y == 0 ? 0 : _wiggleAmplitude.Y * Mathf.Sin(Mathf.Tau * (seconds / _wigglePeriod.Y + _wigglePhaseOffset.Y % 1)),
				_wigglePeriod.Z == 0 ? 0 : _wiggleAmplitude.Z * Mathf.Sin(Mathf.Tau * (seconds / _wigglePeriod.Z + _wigglePhaseOffset.Z % 1))
			);
			
			newTransform = newTransform.Translated(wiggleVector);
		}
		
		SetTransform(newTransform);
		
		if (_pathFollower != null)
		{
			GlobalPosition = _pathFollower.GlobalPosition; 
		}
	}
}
