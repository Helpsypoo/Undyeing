using Godot;
using System;

public partial class MainMenu : Control
{
	private Button StartButton => GetNode<Button>("%StartButton"); 
	
	public override void _Ready()
	{
		StartButton.Pressed += LevelLoader.Instance.LoadNext;
		StartButton.GrabFocus();
	}
}
