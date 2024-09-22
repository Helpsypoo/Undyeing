using Godot;
using System;
using System.Globalization;

public partial class FPSLabel : Label
{
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = Engine.GetFramesPerSecond().ToString(CultureInfo.InvariantCulture);
	}
}
