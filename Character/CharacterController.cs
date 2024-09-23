using Godot;
using System;

public partial class CharacterController : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;
	
	private Camera3D Camera3D => GetViewport().GetCamera3D();
	private AnimationTree AnimationTree => GetNode<AnimationTree>("character/AnimationTree"); 

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("Jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		var cameraRotation = Camera3D.GlobalRotation;
		Vector3 direction = (Camera3D.GlobalBasis.Rotated(Camera3D.GlobalBasis.X, -cameraRotation.X) * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
			
			LookAt(Position + direction);
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}
		AnimationTree.Set("parameters/RunBlend/blend_position", velocity.Length() / Speed);

		if (IsOnFloor())
		{
			AnimationTree.Set("parameters/conditions/OnFloor", true);
			AnimationTree.Set("parameters/conditions/Falling", false);
		}
		else
		{
			AnimationTree.Set("parameters/conditions/OnFloor", false);
			AnimationTree.Set("parameters/conditions/Falling", true);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
