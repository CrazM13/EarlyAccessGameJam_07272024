using Godot;
using System;

public partial class ToolButton : Button {

	[Export] private GameManager gameManager;
	[Export] private ToolBase tool;

	public override void _Ready() {

		ButtonUp += () => {
			gameManager.SetTool(tool);
		};

	}
}
