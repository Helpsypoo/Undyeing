using Godot;
using System;

public partial class LevelLoader : Node
{
	private string[] _levelPaths = new[]
	{
		"res://Levels/01 - Start.tscn",
		"res://Levels/02 - Double Jump.tscn",
		"res://Levels/03 - Dash.tscn",
		"res://Levels/04 - Spring.tscn"
	};

	private int _levelIndex = -1; // Main menu is the beginning, so this increments to zero when loading the first level

	public static LevelLoader Instance;
	public override void _Ready()
	{
		Instance = this;
	}

	public void LoadNext()
	{
		if (_levelIndex == _levelPaths.Length - 1) return;

		_levelIndex++;

		// WinScreen?.QueueFree();
		// _winScreen = null;

		// GetParent().AddChild(_nextLevelScene.Instantiate<Node3D>());
		// QueueFree();

		// var nextScene = ResourceLoader.Load<PackedScene>(_levelPaths[_levelIndex]);

		GetTree().ChangeSceneToFile(_levelPaths[_levelIndex]);
	}
}
