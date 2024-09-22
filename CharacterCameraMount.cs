using Godot;
using System;

public partial class CharacterCameraMount : Node3D
{
	[Export] private CharacterBody3d Character;
	[Export] private float _deathPlaneOffset;
	private Vector3 _offset = new Vector3(0, 1, 0);
	private MeshInstance3D DyeSea => GetNode<MeshInstance3D>("%Dye Sea");
	
	public override void _Process(double delta)
	{
		if (Character.GlobalPosition.Y > DyeSea.GlobalPosition.Y + _deathPlaneOffset)
		{
			Position = Character.Position + _offset;
		}
		
		var cameraInputDirection =
			Input.GetVector("CameraLookLeft", "CameraLookRight", "CameraLookDown", "CameraLookUp");
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