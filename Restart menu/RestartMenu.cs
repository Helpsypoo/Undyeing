using Godot;
using System;
using System.Security.AccessControl;

public partial class RestartMenu : Control
{
	private Button RestartButton => GetNode<Button>("%RestartButton"); 
	private Button NextLevelButton => GetNodeOrNull<Button>("%NextLevelButton"); 
	
	public override void _Ready()
	{
		var mainScene = GetParent<Level>();
		RestartButton.Pressed += mainScene.Restart;
		if (NextLevelButton != null)
		{
			NextLevelButton.Pressed += LevelLoader.Instance.LoadNext;
		}

		RestartButton.GrabFocus();
	}
}
