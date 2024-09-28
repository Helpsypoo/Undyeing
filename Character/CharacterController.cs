using Godot;
using System;
using Microsoft.VisualBasic;

public partial class CharacterController : CharacterBody3D
{
	private const float Speed = 5.0f;
	private const float JumpSpeed = 4.5f;
	[Export] private int _maxExtraJumps = 1;
	private int _extraJumpsUsed;
	
	private const float DashSpeed = 20f;
	[Export] private int _maxDashes = 1;
	[Export] private float _dashDuration = 0.2f;
	private float _lastDashTime = -999; // Big negative to avoid immediate dashes based on dash timing check
	private int _dashesUsed;
	public bool Active = true;
	private Camera3D Camera3D => GetViewport().GetCamera3D();
	private AnimationTree AnimationTree => GetNode<AnimationTree>("character/AnimationTree");

	public override void _Ready()
	{
		GD.Print("Reddy");
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		var onFloor = IsOnFloor();
		
		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		
		var cameraRotation = Camera3D.GlobalRotation;
		Vector3 direction = (Camera3D.GlobalBasis.Rotated(Camera3D.GlobalBasis.X, -cameraRotation.X) * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero && Active)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
			
			LookAt(Position + direction);
		}
		else
		{
			var framesToStop = Active ? 0 : 100; 
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed / framesToStop);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed / framesToStop);
		}
		
		
		// Add the gravity
		if (!onFloor)
		{
			velocity += GetGravity() * (float)delta;
		}
		else
		{
			_extraJumpsUsed = 0;
			_dashesUsed = 0;
		}

		if (Input.IsActionJustPressed("Jump") && Active)
		{
			// Handle Jump.
			if (onFloor)
			{
				velocity.Y = JumpSpeed;
			}
			// Handle Extra Jump
			else if (_extraJumpsUsed < _maxExtraJumps)
			{
				velocity.Y += JumpSpeed;
				_extraJumpsUsed++;
			}
		}
		
		// Process dash
		var seconds = Time.GetTicksMsec() / 1000f;
		var dashing = false;
		if (seconds < _lastDashTime + _dashDuration)
		{
			var dashVelocity = DashSpeed * (Basis * Vector3.Forward);  
			velocity = dashVelocity;
			dashing = true;
		}
		else if (Input.IsActionJustPressed("Dash") && _dashesUsed < _maxDashes && Active)
		{
			var dashVelocity = DashSpeed * (Basis * Vector3.Forward);  
			velocity = dashVelocity;
			_dashesUsed++;
			_lastDashTime = seconds;
			dashing = true;
		}
		
		// Update animation tree parameters based on speed
		AnimationTree.Set("parameters/RunBlend/blend_position", velocity.Length() / Speed);
		AnimationTree.Set("parameters/Airborne/blend_position", (velocity.Y / JumpSpeed) / 2 + 1f);
		if (IsOnFloor())
		{
			AnimationTree.Set("parameters/conditions/OnFloor", true);
			AnimationTree.Set("parameters/conditions/Airborne", false);
		}
		else
		{
			AnimationTree.Set("parameters/conditions/OnFloor", false);
			AnimationTree.Set("parameters/conditions/Airborne", true);
		}

		if (dashing)
		{
			AnimationTree.Set("parameters/conditions/Dashing", true);
			AnimationTree.Set("parameters/conditions/OnFloor", false);
			AnimationTree.Set("parameters/conditions/Airborne", false);
		}
		else
		{
			AnimationTree.Set("parameters/conditions/Dashing", false);
		}
		

		Velocity = velocity;
		MoveAndSlide();
	}
}
