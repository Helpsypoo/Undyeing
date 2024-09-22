using Godot;
using System;

public partial class RestartMenu : Control
{
	private Button RestartButton => GetNode<Button>("%RestartButton"); 
	
	public override void _Ready()
	{
		var mainScene = GetParent<MainScene>();
		RestartButton.Pressed += mainScene.Restart;
	}
}
