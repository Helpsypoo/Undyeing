using Godot;
using System;

public partial class Level : Node3D
{
	private MeshInstance3D DyeSea => GetNode<MeshInstance3D>("%Dye Sea");
	private CharacterController Character => GetNode<CharacterController>("CharacterBody3D");
	private static PackedScene DeathScreenScene => ResourceLoader.Load<PackedScene>("res://Restart menu/DeathMenu.tscn");

	private Control _deathScreen;
	private Control DeathScreen
	{
		get
		{
			if (_deathScreen != null) return _deathScreen;
			_deathScreen = DeathScreenScene.Instantiate<Control>();
			return _deathScreen;
		}
	}
	
	private PackedScene WinScreenScene => ResourceLoader.Load<PackedScene>("res://Restart menu/WinMenu.tscn");

	private Control _winScreen;
	private Control WinScreen
	{
		get
		{
			if (_winScreen != null) return _winScreen;
			_winScreen = WinScreenScene.Instantiate<Control>();
			return _winScreen;
		}
	}

	private Vector3 _initialCharacterPosition;
	public override void _Ready()
	{
		_initialCharacterPosition = Character.Position;

		GetNode<Area3D>("WinPlatform/WinZone").AreaEntered += ShowWinScreen;
	}

	public override void _Process(double delta)
	{
		if (_deathScreen == null && Character.GlobalPosition.Y < DyeSea.GlobalPosition.Y)
		{
			Death();
		}
	}

	public virtual void Death()
	{
		Character.Active = false;
		AddChild(DeathScreen);
	}

	private void ShowWinScreen(Area3D area)
	{
		Character.Active = false;
		AddChild(WinScreen);
	}

	public void Restart()
	{
		DeathScreen?.QueueFree();
		_deathScreen = null;
		
		WinScreen?.QueueFree();
		_winScreen = null;
		
		Character.Position = _initialCharacterPosition;
		Character.Active = true;
	}
}
