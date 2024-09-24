using Godot;
using System;

public partial class CharacterCameraMount : Node3D
{
	[Export] private CharacterController Character;
	[Export] private float _deathPlaneOffset;
	// [Export] private float _followDampingFactor = 0.5f;
	[Export] private float _followStrength = 1;
	private float _maxCameraSpeed = 100;
	private Vector3 _offset = new Vector3(0, 1, 0);

	private Vector3 _velocity;
	private Vector3 _acceleration;

	public override void _Ready()
	{
		var fps = (int) MathF.Round(DisplayServer.ScreenGetRefreshRate()); // How to get this rather than hard-coding?
		Engine.PhysicsTicksPerSecond = fps;
		Engine.MaxFps = fps;
	}

	private MeshInstance3D DyeSea => GetNode<MeshInstance3D>("%Dye Sea");
	
	public override void _PhysicsProcess(double delta)
	{
		if (Character.GlobalPosition.Y > DyeSea.GlobalPosition.Y + _deathPlaneOffset)
		{
			var flelta = (float)delta;
			
			var separation = Character.GlobalPosition + _offset - GlobalPosition;
			var length = separation.Length();
			var speed = Mathf.Pow(length, 4) * _followStrength;
			speed = Mathf.Min(speed, _maxCameraSpeed); // Cap camera speed
			var updateDistance = speed * flelta;
			GlobalPosition +=  separation.Normalized() * updateDistance;
		}
		
		var cameraInputDirection = Input.GetVector("CameraLookLeft", "CameraLookRight", "CameraLookDown", "CameraLookUp");
		if (cameraInputDirection != Vector2.Zero)
		{
			const float deadZone = 0.1f;
			cameraInputDirection = new Vector2(
				Mathf.Abs(cameraInputDirection.X) > deadZone ? cameraInputDirection.X : 0,
				Mathf.Abs(cameraInputDirection.Y) > deadZone ? cameraInputDirection.Y : 0
			);
			
			const float cameraHorizontalRotationSpeed = 3f;
			const float cameraVerticalRotationSpeed = 3f;
			
			var nextYaw = RotationDegrees.Y - cameraInputDirection.X * cameraHorizontalRotationSpeed;
			
			const float maxPitchDegrees = -70; // Negative mean camera go up
			const float minPitchDegrees = 10;
			var nextPitch = RotationDegrees.X - cameraInputDirection.Y * cameraVerticalRotationSpeed;
			nextPitch = Mathf.Max(maxPitchDegrees, nextPitch);
			nextPitch = Mathf.Min(minPitchDegrees, nextPitch);
			RotationDegrees = new Vector3(nextPitch, nextYaw, 0);
		}
	}
}